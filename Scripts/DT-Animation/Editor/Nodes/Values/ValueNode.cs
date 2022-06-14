// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    /// <summary>
    /// A Node for inputing raw values.
    /// </summary>
    public abstract class ValueNode<T> : DTAnimatorNode
    {
        public ValueNode(ValueVertex<T> vertex) : base(vertex)
        {
            this.Q<VisualElement>("title").RemoveFromHierarchy();
            AddToClassList("value-node");
            outputContainer.name = "value-output";

            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(T), Port.Capacity.Multi);
            VisualElement field = CreateField();
            if(!(vertex is GetterVertex<T> || typeof(T) == typeof(bool))) field.AddToClassList("value-field");
        }

        protected abstract VisualElement CreateField();
    }
}
