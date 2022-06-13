using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    /// <summary>
    /// A node for calling an exposed method to retrieve a value.
    /// </summary>
    public class GetterNode<T> : ValueNode<T>
    {
        public GetterNode(GetterVertex<T> vertex) : base(vertex)
        {
            outputContainer.Add(new Label(typeof(T).Name));
            AddToClassList("getter-node");
        }

        protected override VisualElement CreateField()
        {
            MethodField field = new MethodField();
            GetterVertex<T> vertex = (GetterVertex<T>)Vertex;
            field.SetValueWithoutNotify(vertex.ExposedMethod);
            field.RegisterValueChangedCallback(evt => vertex.ExposedMethod = evt.newValue);
            field.ReturnType = typeof(T);
            extensionContainer.Add(field);
            RefreshExpandedState();
            return field;
        }
    }
}
