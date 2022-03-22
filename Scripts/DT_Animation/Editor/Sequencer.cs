using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using NoMoney.NoMoneyEditor;

namespace NoMoney.DTAnimation
{
    public class Sequencer : EditorWindow
    {
        private SequencerView _sequencerView;
        private List<Vertex> _vertices;

        private static SerializedObject s_animator;

        public static void ShowWindow(SerializedObject animator)
        {
            s_animator = animator;
            GetWindow<Sequencer>("DT Sequencer");
        }

        private void OnEnable()
        {
            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("DTSequencer"));
            _vertices = (List<Vertex>)s_animator.FindProperty("_vertices").GetValue();

            CreateSequencerView();
            CreateToolbar();
        }

        private void OnDisable()
        {
            rootVisualElement.Remove(_sequencerView);
        }

        private void CreateSequencerView()
        {
            _sequencerView = new SequencerView(_vertices);
            _sequencerView.StretchToParentSize();
            rootVisualElement.Add(_sequencerView);
        }

        private void CreateToolbar()
        {
            Toolbar toolbar = new Toolbar();
            
            Button saveButton = new Button(Save)
            {
                text = "Save"
            };
            toolbar.Add(saveButton);

            Button clearButton = new Button(Clear)
            {
                text = "Clear"
            };
            toolbar.Add(clearButton);

            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            s_animator.FindProperty("_vertices").SetValue(_vertices);
            List<SequenceVertex> seqVertices = new List<SequenceVertex>();
            foreach(Vertex v in _vertices)
            {
                if(v is SequenceVertex seqVertex)
                {
                    seqVertices.Add(seqVertex);
                }
            }
            s_animator.FindProperty("_seqVertices").SetValue(seqVertices);
            if (!UniqSequences(seqVertices))
            {
                Debug.LogWarning("Duplicate Sequence Names Found: Make sure sequence names are unique.");
            }
            Debug.Log("Saved Graph");
        }
    
        private void Clear()
        {
            _sequencerView.ClearView();
            DTAnimatorInspector.ClearModel(s_animator);
        }

        private bool UniqSequences(List<SequenceVertex> seqVertices)
        {
            foreach(SequenceVertex v1 in seqVertices)
            {
                foreach(SequenceVertex v2 in seqVertices)
                {
                    if(v1 == v2)
                    {
                        continue;
                    }
                    if(v1.SequenceName == v2.SequenceName)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
