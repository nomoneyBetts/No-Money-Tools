using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;

namespace NoMoney.DTAnimation
{
    public class SequencerView : GraphView
    {
        private readonly List<Vertex> _vertices;
        private readonly List<GroupVertex> _groups;
        private readonly Transform _defaultTarget;

        public SequencerView(List<Vertex> vertices, List<GroupVertex> groups, Transform defaultTarget)
        {
            _vertices = vertices;
            _groups = groups;
            _defaultTarget = defaultTarget;

            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);
            
            this.AddManipulator(new ContentDragger());
            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());

            GridBackground background = new GridBackground();
            Insert(0, background);
            background.StretchToParentSize();

            GenerateView();

            graphViewChanged += OnViewChanged;
        }

        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            Vector2 pos = viewTransform.matrix.inverse.MultiplyPoint(evt.localMousePosition);
            TypeCache.TypeCollection vertexTypes = TypeCache.GetTypesDerivedFrom<Vertex>();
            foreach(Type type in vertexTypes)
            {
                if(type.IsAbstract)
                {
                    continue;
                }

                NodeMenuDisplayAttribute attribute;
                string display = (attribute = type.GetCustomAttribute<NodeMenuDisplayAttribute>()) == null ?
                    type.Name : 
                    attribute.Display;

                evt.menu.AppendAction(display, a => CreateNode(CreateVertex(type, pos)));
            }
        }

        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            List<Port> compatiblePorts = new List<Port>();
            ports.ForEach(port =>
            {
                if (CompatiblePorts(startPort, port))
                {
                    compatiblePorts.Add(port);
                }
            });

            return compatiblePorts;
        }

        public void ClearView()
        {
            graphElements.ForEach(e => RemoveElement(e));
        }

        #region Node Creation
        public Node CreateNode(Vertex vertex)
        {
            Type vertexType = vertex.GetType();
            Type type = Type.GetType(vertexType.FullName.Replace("Vertex", "Node"));
            Node node = (Node)Activator.CreateInstance(type, new object[1] { vertex });
            AddElement(node);
            return node;
        }

        public Vertex CreateVertex(Type type, Vector2 pos)
        {
            Vertex vertex = (Vertex)ScriptableObject.CreateInstance(type);
            vertex.NodePosition = pos;
            _vertices.Add(vertex);
            if (vertex is TweenVertex v) v.DefaultTarget = _defaultTarget;
            return vertex;
        }

        public void CreateGroup()
        {
            List<DTAnimatorNode> nodes = new List<DTAnimatorNode>(selection.Count);
            selection.ForEach(s => { if (s is DTAnimatorNode n) nodes.Add(n); });
            if(nodes.Count > 0)
            {
                GroupVertex gv = ScriptableObject.CreateInstance<GroupVertex>();
                _groups.Add(gv);
                GroupNode group = new GroupNode(gv);
                group.AddElements(nodes);
                nodes.ForEach(n => gv.Group.Add(n.Vertex));

                TextField titleField = group.Q<TextField>("titleField");
                titleField.RegisterValueChangedCallback(evt => gv.Title = evt.newValue);
                titleField.Focus();

                AddElement(group);
            }
        }

        public void Ungroup()
        {
            List<GroupNode> groups = new List<GroupNode>(selection.Count);
            selection.ForEach(s => { if (s is GroupNode g) groups.Add(g); });
            groups.ForEach(RemoveGroup);
        }

        private void RemoveGroup(GroupNode group)
        {
            List<GraphElement> groupElements = new List<GraphElement>(group.containedElements);
            group.RemoveElementsWithoutNotification(groupElements);
            _groups.Remove(group.Vertex);
            group.Vertex.DestroyVertex();
            RemoveElement(group);
        }

        public void ShowMenuNode(Vector2 pos, string baseType) => AddElement(new MenuNode(this, pos, baseType));
        #endregion

        private void GenerateView()
        {
            // First pass - create nodes
            for(int i = 0; i < _vertices.Count; i++)
            {
                CreateNode(_vertices[i]);
            }

            // Second pass - create the edges
            foreach (Vertex vertex in _vertices)
            {
                DTAnimatorNode outNode = FindNode(vertex);
                List<Port> outPorts = outNode.GetPorts();
                foreach (Connection cnx in vertex.Outputs)
                {
                    DTAnimatorNode inNode = FindNode(cnx.Cnx);
                    List<Port> inPorts = inNode.GetPorts();

                    Port outPort = outPorts.Find(p => p.portName == cnx.PortName);
                    Port inPort = inPorts.Find(p => p.portName == cnx.CnxPort);
                    Edge edge = new Edge()
                    {
                        output = outPort,
                        input = inPort
                    };
                    inPort.Connect(edge);
                    outPort.Connect(edge);
                    AddElement(edge);

                    // If the input node is an elbow, it must be updated
                    if (inNode is ElbowNode elbow) elbow.ConnectOutput(outNode, this);
                }
            }

            // Third pass - create the groups
            foreach(GroupVertex group in _groups)
            {
                GroupNode gn = new GroupNode(group);
                gn.title = group.Title;
                group.Group.ForEach(v => gn.AddElement(FindNode(v)));
                AddElement(gn);
            }
        }
        
        public DTAnimatorNode FindNode(Vertex vertex)
        {
            DTAnimatorNode node = null;
            nodes.ForEach(n =>
            {
                if(n is DTAnimatorNode animNode && animNode.Vertex == vertex)
                {
                    node = animNode;
                }
            });
            return node;
        }

        private bool CompatiblePorts(Port startPort, Port endPort)
        {
            return !(startPort == endPort || startPort.node == endPort.node || startPort.direction == endPort.direction);
        }

        private GraphViewChange OnViewChanged(GraphViewChange change)
        {
            if(change.movedElements != null)
            {
                change.movedElements.ForEach(e =>
                {
                    if(e is DTAnimatorNode animNode)
                    {
                        Vector2 pos = e.GetPosition().position;
                        animNode.Vertex.NodePosition = pos;
                    }
                });
            }

            if(change.elementsToRemove != null)
            {
                change.elementsToRemove.ForEach(e =>
                {
                    if (e is DTAnimatorNode animNode)
                    {
                        _vertices.Remove(animNode.Vertex);
                        animNode.Vertex.DestroyVertex();
                    }
                    else if(e is Edge edge && edge.input.node is DTAnimatorNode inNode && edge.output.node is DTAnimatorNode outNode)
                    {
                        inNode.Vertex.DisconnectVertex(edge.input.portName, edge.output.portName, outNode.Vertex);
                        outNode.Vertex.DisconnectVertex(edge.output.portName, edge.input.portName, inNode.Vertex);

                        // Elbows must be updated on disconnections
                        if (inNode is ElbowNode eIn) eIn.DisconnectOutput(this);
                    }
                    else if(e is GroupNode gn)
                    {
                        RemoveGroup(gn);
                    }
                });
            }
            
            if(change.edgesToCreate != null)
            {
                foreach(Edge edge in change.edgesToCreate)
                {
                    if(edge.input.node is DTAnimatorNode inNode && edge.output.node is DTAnimatorNode outNode)
                    {
                        inNode.Vertex.ConnectVertex(edge.input.portName, edge.output.portName, outNode.Vertex);
                        outNode.Vertex.ConnectVertex(edge.output.portName, edge.input.portName, inNode.Vertex);

                        // Elbow Nodes must be updated on connections
                        if (inNode is ElbowNode eIn) eIn.ConnectOutput(outNode, this);
                    }
                };
            }

            return change;
        }
    }
}
