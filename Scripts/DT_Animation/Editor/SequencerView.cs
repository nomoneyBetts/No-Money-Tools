using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor;
using DG.Tweening;
using NoMoney.NoMoneyEditor;

namespace NoMoney.DTAnimation
{
    public class SequencerView : GraphView
    {
        private readonly List<Vertex> _vertices;

        public SequencerView(List<Vertex> vertices)
        {
            _vertices = vertices;

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
            TypeCache.TypeCollection types = TypeCache.GetTypesDerivedFrom<Vertex>();
            foreach(Type type in types)
            {
                if(type.IsAbstract)
                {
                    continue;
                }
                string display = type.Name.Replace("Vertex", "");
                // "Value" looks clunky when attached to value vertices.
                display = display.Replace("Value", "");
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
            Node node;
            string type = vertex.GetType().Name;
            switch (type)
            {
                case "FloatValueVertex":
                    node = new ValueNode<float>((FloatValueVertex)vertex);
                    break;
                case "Vector3ValueVertex":
                    node = new ValueNode<Vector3>((Vector3ValueVertex)vertex);
                    break;
                case "EaseValueVertex":
                    node = new ValueNode<Ease>((EaseValueVertex)vertex);
                    break;
                case "SequenceVertex":
                    node = new SequenceNode((SequenceVertex)vertex);
                    break;
                case "JoinVertex":
                    node = new JoinNode((JoinVertex)vertex);
                    break;
                case "MoveVertex":
                    node = new MoveNode((MoveVertex)vertex);
                    break;
                case "RotateVertex":
                    node = new RotateNode((RotateVertex)vertex);
                    break;
                default:
                    Debug.LogError("Unknown Type: " + type);
                    return null;
            }
            AddElement(node);
            return node;
        }

        public Vertex CreateVertex(Type type, Vector2 pos)
        {
            Vertex vertex;
            switch (type.Name)
            {
                case "FloatValueVertex":
                    vertex = ScriptableObject.CreateInstance<FloatValueVertex>();
                    break;
                case "Vector3ValueVertex":
                    vertex = ScriptableObject.CreateInstance<Vector3ValueVertex>();
                    break;
                case "EaseValueVertex":
                    vertex = ScriptableObject.CreateInstance<EaseValueVertex>();
                    break;
                case "SequenceVertex":
                    vertex = ScriptableObject.CreateInstance<SequenceVertex>();
                    break;
                case "JoinVertex":
                    vertex = ScriptableObject.CreateInstance<JoinVertex>();
                    break;
                case "MoveVertex":
                    vertex = ScriptableObject.CreateInstance<MoveVertex>();
                    break;
                case "RotateVertex":
                    vertex = ScriptableObject.CreateInstance<RotateVertex>();
                    break;
                default:
                    Debug.LogError("Unknown Type: " + type.Name);
                    return null;
            }
            vertex.NodePosition = pos;
            _vertices.Add(vertex);
            return vertex;
        }
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
                foreach (Connection cnx in vertex.GetOutputs())
                {
                    DTAnimatorNode inNode = FindNode(cnx.CnxVertex);
                    List<Port> inPorts = inNode.GetPorts();

                    Port outPort = outPorts.Find(p => p.portName == cnx.PortName);
                    Port inPort = inPorts.Find(p => p.portName == cnx.CnxPort);
                    Edge edge = new Edge()
                    {
                        output = outPort,
                        input = inPort
                    };
                    outPort.Connect(edge);
                    inPort.Connect(edge);
                    AddElement(edge);
                }
            }
        }

        private DTAnimatorNode FindNode(Vertex vertex)
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
                    }
                });
            }
            
            if(change.edgesToCreate != null)
            {
                change.edgesToCreate.ForEach(edge =>
                {
                    if(edge.input.node is DTAnimatorNode inNode && edge.output.node is DTAnimatorNode outNode)
                    {
                        inNode.Vertex.ConnectVertex(edge.input.portName, edge.output.portName, outNode.Vertex);
                        outNode.Vertex.ConnectVertex(edge.output.portName, edge.input.portName, inNode.Vertex);
                    }
                });
            }

            return change;
        }
    }
}
