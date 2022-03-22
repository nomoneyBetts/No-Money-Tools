using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using NoMoney.NoMoneyEditor;

namespace NoMoney.DTAnimation
{
    [CustomEditor(typeof(DTAnimator))]
    public class DTAnimatorInspector : Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            VisualElement inspector = new VisualElement();

            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("DTAnimator");
            visualTree.CloneTree(inspector);

            inspector.styleSheets.Add(Resources.Load<StyleSheet>("DTAnimator"));

            Button editorButton = inspector.Q<Button>("open");
            editorButton.clicked += () => Sequencer.ShowWindow(serializedObject);

            Button clearButton = inspector.Q<Button>("clear");
            clearButton.clicked += () => ClearModel(serializedObject);

            return inspector;
        }

        public static void ClearModel(SerializedObject serializedObject)
        {
            List<Vertex> vertices = (List<Vertex>)serializedObject.FindProperty("_vertices").GetValue();
            foreach(Vertex vertex in vertices)
            {
                if(vertex == null)
                {
                    continue;
                }
                // Destroy out connections
                foreach(Connection cnx in vertex.GetOutputs())
                {
                    cnx.DestroyCnx();
                }

                // Destroy the vertex
                vertex.DestroyVertex();
            }
            serializedObject.FindProperty("_vertices").SetValue(new List<Vertex>());
            serializedObject.FindProperty("_seqVertices").SetValue(new List<SequenceVertex>());
        }
    }
}
