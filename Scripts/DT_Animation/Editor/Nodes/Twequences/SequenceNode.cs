using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using NoMoney.NoMoneyEditor;

namespace NoMoney.DTAnimation
{
    public class SequenceNode : TwequenceNode
    {        
        public SequenceNode(SequenceVertex seqVertex) : base(seqVertex)
        {
            title = "Sequence Node";

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
