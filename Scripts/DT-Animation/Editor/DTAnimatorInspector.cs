// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

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
            editorButton.clicked += () => Sequencer.ShowWindow(serializedObject, this);

            Button clearButton = inspector.Q<Button>("clear");
            clearButton.clicked += () => ClearModel();

            return inspector;
        }

        public void ClearModel()
        {
            List<Vertex> vertices = (List<Vertex>)serializedObject.FindProperty("_vertices").GetValue();
            foreach(Vertex vertex in vertices)
            {
                if (vertex == null) continue;

                // Destroy out connections
                foreach (Connection cnx in vertex.Outputs)
                {
                    if (cnx == null) continue;
                    cnx.DestroyCnx();
                }

                // Destroy the vertex
                vertex.DestroyVertex();
            }

            List<GroupVertex> groups = (List<GroupVertex>)serializedObject.FindProperty("_groups").GetValue();
            foreach(GroupVertex group in groups)
            {
                if (group == null) continue;
                group.DestroyVertex();
            }

            ((List<Vertex>)serializedObject.FindProperty("_vertices").GetValue()).Clear();
            ((StringVertexDict)serializedObject.FindProperty("_seqVertices").GetValue()).Clear();
            ((List<GroupVertex>)serializedObject.FindProperty("_groups").GetValue()).Clear();
        }
    }
}
