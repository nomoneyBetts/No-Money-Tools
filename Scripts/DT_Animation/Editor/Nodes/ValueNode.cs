using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DG.Tweening;

namespace NoMoney.DTAnimation
{
    /// <summary>
    /// A Node for inputing raw values.
    /// </summary>
    public class ValueNode<T> : DTAnimatorNode
    {
        public ValueNode(ValueVertex<T> vertex) : base(vertex)
        {
            this.Q<VisualElement>("title").RemoveFromHierarchy();
            AddToClassList("value-node");
            outputContainer.name = "value-output";

            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(T), Port.Capacity.Multi);
            CreateField();
        }

        private void CreateField()
        {
            if(typeof(T) == typeof(float))
            {
                ValueVertex<float> vertex = (ValueVertex<float>)Vertex;
                FloatField field = new FloatField();
                field.SetValueWithoutNotify(vertex.Value);
                field.RegisterValueChangedCallback(evt => vertex.Value = evt.newValue);
                outputContainer.Add(field);
            }
            else if(typeof(T) == typeof(Vector3))
            {
                ValueVertex<Vector3> vertex = (ValueVertex<Vector3>)Vertex;
                Vector3Field field = new Vector3Field();
                field.SetValueWithoutNotify(vertex.Value);
                field.RegisterValueChangedCallback(evt => vertex.Value = evt.newValue);
                outputContainer.Add(field);
            }
            else if(typeof(T) == typeof(Ease))
            {
                ValueVertex<Ease> vertex = (ValueVertex<Ease>)Vertex;
                EnumField field = new EnumField(vertex.Value);
                field.RegisterValueChangedCallback(evt => vertex.Value = (Ease)evt.newValue);
                outputContainer.Add(field);
            }
        }
    }
}
