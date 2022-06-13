using UnityEditor.Experimental.GraphView;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class MenuNode : Node
    {
        public MenuNode(SequencerView view, Vector2 pos, string baseType)
        {
            title = $"{baseType} Nodes";           
            this.Q<VisualElement>("top").RemoveFromHierarchy();
            SetPosition(new Rect(pos, Vector2.zero));
            RegisterCallback<MouseLeaveEvent>(evt => view.RemoveElement(this));

            TypeCache.TypeCollection vertexTypes = TypeCache.GetTypesDerivedFrom<Vertex>();
            foreach (Type type in vertexTypes)
            {
                if (type.IsAbstract) continue;

                NodeMenuDisplayAttribute attribute;
                string display = (attribute = type.GetCustomAttribute<NodeMenuDisplayAttribute>()) == null ?
                    type.Name :
                    attribute.Display;
                if (!display.Contains(baseType)) continue;

                string[] heirarchy = display.Split('/');
                VisualElement current = extensionContainer;
                bool found = false;
                for(int i = 0; i < heirarchy.Length - 1; i++)
                {
                    found = heirarchy[i].Contains(baseType);
                    if (!found) continue;

                    VisualElement next = current.Q<VisualElement>(heirarchy[i]);
                    if (next == null)
                    {
                        next = new Foldout() { name = heirarchy[i], text = heirarchy[i], value = false };
                        current.Add(next);
                    };
                    current = next;
                }
                current.Add(new Button(() => CreateNode(type))
                {
                    text = heirarchy[^1]
                });
            }

            RefreshExpandedState();

            void CreateNode(Type type)
            {
                view.CreateNode(view.CreateVertex(type, pos));
                view.RemoveElement(this);
            }
        }
    }
}
