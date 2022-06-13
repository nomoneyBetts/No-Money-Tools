using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class SequenceNode : DTAnimatorNode
    {
        public SequenceNode(SequenceVertex seqVertex) : base(seqVertex)
        {
            title = "Sequence Node";
            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(Node));
            CreateNodePort("Events", Orientation.Horizontal, Direction.Input, typeof(ExposedEvent));

            TextField nameField = new TextField();
            nameField.SetValueWithoutNotify(seqVertex.SequenceName);
            nameField.SetPlaceHolderText("Name");
            nameField.RegisterValueChangedCallback(evt => seqVertex.SequenceName = evt.newValue);

            Toggle dynamicGen = new Toggle("Dynamic Generation");
            dynamicGen.SetValueWithoutNotify(seqVertex.DynamicGeneration);
            dynamicGen.RegisterValueChangedCallback(evt => seqVertex.DynamicGeneration = evt.newValue);

            extensionContainer.Add(nameField);
            extensionContainer.Add(dynamicGen);

            RefreshExpandedState();
        }
    }
}
