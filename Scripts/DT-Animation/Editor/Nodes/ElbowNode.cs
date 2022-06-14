// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    public class ElbowNode : DTAnimatorNode
    {
        public string Text
        {
            get
            {
                Label label = outputContainer.Q<Label>(OutputLabel);
                return label == null ? null : label.text;
            }
            set
            {
                Label label = outputContainer.Q<Label>(OutputLabel);
                if (label == null)
                {
                    label = new Label(value);
                    label.name = OutputLabel;
                    outputContainer.Add(label);
                }
                else
                {
                    label.text = value;
                }
            }
        }

        private const string OutputLabel = "output-label";

        public ElbowNode(ElbowVertex vertex) : base(vertex)
        {
            this.Q<VisualElement>("title").RemoveFromHierarchy();
            CreateNodePort("Input", Orientation.Horizontal, Direction.Input, typeof(Node));
            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(Node), Port.Capacity.Multi);
        }

        /// <summary>
        /// On connection, update me if I'm the output node with the incoming input node.
        /// </summary>
        public void ConnectOutput(DTAnimatorNode node, SequencerView view)
        {
            string text = node is ElbowNode elbow ?
                elbow.Text :
                node.GetType().Name.Replace("Value", "").Replace("Node", "");

            Propogate(text, view);
        }

        /// <summary>
        /// On disconnect, update me if I'm the output node.
        /// </summary>
        public void DisconnectOutput(SequencerView view) => Propogate(null, view);

        /// <summary>
        /// Propogate updates down the elbow chain
        /// </summary>
        /// <param name="text"></param>
        public void Propogate(string text, SequencerView view)
        {
            Text = text;
            ElbowVertex vertex = (ElbowVertex)Vertex;
            foreach (Connection cnx in vertex.Outputs)
            {
                if (cnx.Cnx is ElbowVertex link)
                {
                    ((ElbowNode)view.FindNode(link)).Propogate(Text, view);
                }
            }
        }
    }
}
