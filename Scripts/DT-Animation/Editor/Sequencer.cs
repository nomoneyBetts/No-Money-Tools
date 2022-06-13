using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace NoMoney.DTAnimation
{
    public class Sequencer : EditorWindow
    {
        private SequencerView _sequencerView;
        private List<Vertex> _vertices;
        private List<GroupVertex> _groups;

        private static SerializedObject s_animator;
        private static DTAnimatorInspector s_inspector;

        private Vector2 _mousePos;

        public static void ShowWindow(SerializedObject animator, DTAnimatorInspector inspector)
        {
            s_animator = animator;
            s_inspector = inspector;
            GetWindow<Sequencer>("DT Sequencer");
        }

        private void OnEnable()
        {
            if (s_animator == null) return;

            rootVisualElement.styleSheets.Add(Resources.Load<StyleSheet>("DTSequencer"));
            _vertices = (List<Vertex>)s_animator.FindProperty("_vertices").GetValue();
            if (_vertices == null) _vertices = new List<Vertex>();
            _groups = (List<GroupVertex>)s_animator.FindProperty("_groups").GetValue();
            if (_groups == null) _groups = new List<GroupVertex>();

            rootVisualElement.RegisterCallback<MouseMoveEvent>(evt => _mousePos = evt.localMousePosition);
            rootVisualElement.RegisterCallback<KeyDownEvent>(evt =>
            {
                if (evt.modifiers == EventModifiers.Control)
                {
                    switch (evt.keyCode)
                    {
                        // Menus
                        case KeyCode.P:
                            _sequencerView.ShowMenuNode(_mousePos, "Renderer");
                            evt.StopPropagation();
                            break;
                        case KeyCode.C:
                            _sequencerView.ShowMenuNode(_mousePos, "Camera");
                            evt.StopPropagation();
                            break;
                        case KeyCode.M:
                            // Menu for Material Tweens
                            _sequencerView.ShowMenuNode(_mousePos, "Material");
                            evt.StopPropagation();
                            break;
                        case KeyCode.A:
                            // Menu for Audio Tweens
                            _sequencerView.ShowMenuNode(_mousePos, "Audio");
                            evt.StopPropagation();
                            break;
                        case KeyCode.T:
                            // Menu for transform tweens
                            _sequencerView.ShowMenuNode(_mousePos, "Transform");
                            evt.StopPropagation();
                            break;
                        case KeyCode.R:
                            // Menu for Rigidbody tweens
                            _sequencerView.ShowMenuNode(_mousePos, "Rigidbody");
                            evt.StopPropagation();
                            break;

                        // Grouping
                        case KeyCode.G:
                            // Group
                            _sequencerView.CreateGroup();
                            evt.StopPropagation();
                            break;
                        case KeyCode.U:
                            // Ungroup
                            _sequencerView.Ungroup();
                            evt.StopPropagation();
                            break;

                        // Values
                        case KeyCode.F:
                            // Float Value
                            _sequencerView.CreateNode(_sequencerView.CreateVertex(typeof(FloatValueVertex), _mousePos));
                            evt.StopPropagation();
                            break;
                        case KeyCode.I:
                            // Int Value
                            _sequencerView.CreateNode(_sequencerView.CreateVertex(typeof(IntValueVertex), _mousePos));
                            evt.StopPropagation();
                            break;
                        case KeyCode.E:
                            // Ease Value
                            _sequencerView.CreateNode(_sequencerView.CreateVertex(typeof(EaseValueVertex), _mousePos));
                            evt.StopPropagation();
                            break;
                        case KeyCode.B:
                            // Bool value
                            _sequencerView.CreateNode(_sequencerView.CreateVertex(typeof(BoolValueVertex), _mousePos));
                            evt.StopPropagation();
                            break;
                        case KeyCode.S:
                            // String value
                            _sequencerView.CreateNode(_sequencerView.CreateVertex(typeof(StringValueVertex), _mousePos));
                            evt.StopPropagation();
                            break;
                        case KeyCode.V:
                            // Vector3 value
                            _sequencerView.CreateNode(_sequencerView.CreateVertex(typeof(Vector3ValueVertex), _mousePos));
                            evt.StopPropagation();
                            break;
                    }
                }
            });

            CreateSequencerView();
            CreateToolbar();
        }

        private void CreateSequencerView()
        {
            _sequencerView = new SequencerView(_vertices, _groups, ((DTAnimator)s_animator.targetObject).transform);
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

            Button groupButton = new Button(_sequencerView.CreateGroup)
            {
                text = "Group"
            };
            toolbar.Add(groupButton);

            Button ungroupButton = new Button(_sequencerView.Ungroup)
            {
                text = "Ungroup"
            };
            toolbar.Add(ungroupButton);

            rootVisualElement.Add(toolbar);
        }

        private void Save()
        {
            // Collect Sequence Vertices
            StringVertexDict seqVertices = new StringVertexDict();
            foreach (Vertex v in _vertices)
            {
                if(v is SequenceVertex seqVertex)
                {
                    if(string.IsNullOrEmpty(seqVertex.SequenceName))
                    {
                        Debug.LogError("Failed to Save: A sequence's name is null or empty.");
                        return;
                    }
                    if(!seqVertices.TryAdd(seqVertex.SequenceName, seqVertex))
                    {
                        Debug.LogError("Failed to Save: Duplicate sequence names exist.");
                        return;
                    }
                }
            }
            s_animator.FindProperty("_seqVertices").SetValue(seqVertices);
            s_animator.FindProperty("_vertices").SetValue(_vertices);
            s_animator.FindProperty("_groups").SetValue(_groups);

            Debug.Log("Saved Graph");
        }
    
        private void Clear()
        {
            _sequencerView.ClearView();
            s_inspector.ClearModel();
        }
    }
}
