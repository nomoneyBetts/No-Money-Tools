using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using Object = UnityEngine.Object;

namespace NoMoney
{
    [CustomEditor(typeof(PathManager))]
    public class PathManagerInspector : Editor
    {
        private StringCurveDict _curves;
        private PathCurve _selectedCurve;
        private VisualElement _inspector;
        private Vector3ListField _waypoints;
        private readonly List<IDisposable> _eventTokens = new List<IDisposable>();

        private Vector3 _targetPos, _targetScale;
        private Quaternion _targetRot;

        private int _hoverPointIndex = -1;
        private int _selectedPointIndex = -1;
        private bool _drawPosHandle;

        #region Element Names
        private const string SelectedCurveProp = "_selectedCurve";
        private const string PathSettingsWrapper = "path-settings-wrapper";
        private const string ManagementWrapper = "management-wrapper";
        private const string WaypointsWrapper = "waypoints-wrapper";
        private const string EditorSettingsWrapper = "editor-settings-wrapper";
        private const string ColorsWrapper = "editor-colors-wrapper";
        #endregion

        public override VisualElement CreateInspectorGUI()
        {
            _inspector = new VisualElement();
            _curves = (StringCurveDict)serializedObject.FindProperty("_curves").GetValue();
            _selectedCurve = (PathCurve)serializedObject.FindProperty("_selectedCurve").GetValue();

            SetInitialTransform();

            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("PathManagerInspector");
            visualTree.CloneTree(_inspector);
            _inspector.styleSheets.Add(Resources.Load<StyleSheet>("PathManagerInspector"));
            _waypoints = (Vector3ListField)_inspector.Q<VisualElement>(WaypointsWrapper).ElementAt(0);

            // Wire buttons
            VisualElement managementWrapper = _inspector.Q<VisualElement>(ManagementWrapper);
            Button addButton = (Button)managementWrapper.ElementAt(1).ElementAt(0);
            addButton.clicked += AddPath;
            Button removeButton = (Button)managementWrapper.ElementAt(1).ElementAt(1);
            removeButton.clicked += RemovePath;
            Button clearWaypoints = (Button)_inspector.Q<VisualElement>(WaypointsWrapper).ElementAt(1);
            clearWaypoints.clicked += () =>
            {
                _selectedCurve.ClearWaypoints();
                _waypoints.SetValueWithoutNotify(null);
                SceneView.RepaintAll();
            };

            HideCurveSpecificElements();
            PopulateInspector();

            return _inspector;

            #region Add, Remove Paths
            void AddPath()
            {
                _selectedCurve = CreateInstance<PathCurve>();
                _selectedCurve.Target = ((PathManager)target).transform;
                SetValue(SelectedCurveProp, _selectedCurve);
                _curves.Add(_selectedCurve.PathName, _selectedCurve);
                PopulateInspector();
                UpdateSelector(_selectedCurve.PathName);
            }
            void RemovePath()
            {
                if (_selectedCurve == null) return;
                _curves.Remove(_selectedCurve.PathName);
                DestroyImmediate(_selectedCurve);
                _selectedCurve = null;
                SetValue(SelectedCurveProp, _selectedCurve);
                UpdateSelector("NONE");
            }
            #endregion
        }

        private void OnSceneGUI()
        {
            if (_selectedCurve == null) return;
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                    if (e.keyCode == KeyCode.Delete)
                    {
                        if(_selectedPointIndex != -1 && _selectedPointIndex < _selectedCurve.Waypoints.Count)
                        {
                            _selectedCurve.RemoveWaypoint(_selectedPointIndex);
                            if (_selectedPointIndex == _hoverPointIndex) _hoverPointIndex = -1;
                            _selectedPointIndex = -1;
                            _waypoints.SetValueWithoutNotify(_selectedCurve.Waypoints);
                        }
                        e.Use();
                    }
                    else if(e.keyCode == KeyCode.Escape)
                    {
                        _selectedPointIndex = -1;
                        _drawPosHandle = false;
                        e.Use();
                    }
                    break;
                case EventType.MouseDown:
                    if (e.shift && !e.control && !e.alt && _hoverPointIndex == -1)
                    {
                        AddWaypoint();
                        e.Use();
                    }
                    else if (_hoverPointIndex == _selectedPointIndex && _selectedPointIndex != -1)
                    {
                        _drawPosHandle = true;
                    }
                    else if(_hoverPointIndex != -1)
                    {
                        _drawPosHandle = false;
                        _selectedPointIndex = _hoverPointIndex; 
                    }
                    break;
                case EventType.MouseMove:
                    MouseOverPoint();
                    break;
            }

            if (_selectedCurve.MoveRelative)
            {
                Transform trans = ((PathManager)target).transform;
                MoveWaypoints(trans.position - _targetPos);
                ScaleWaypoints(trans.localScale - _targetScale);
                RotateWaypoints(trans.rotation * Quaternion.Inverse(_targetRot));
                _targetPos = trans.position;
                _targetScale = trans.localScale;
                _targetRot = trans.rotation;
            }

            DrawCurve();
            DrawCBControls();
            DrawArrows();
            DrawWaypoints();
        }

        #region Inspector Updates
        private void PopulateInspector()
        {
            if (_selectedCurve == null) UpdateSelector();
            else UpdateSelector(_selectedCurve.PathName);

            if(_selectedCurve == null)
            {
                _eventTokens.ForEach(token => token.Dispose());
                return;
            }

            EventCallback<ChangeEvent<string>> stringChange;
            EventCallback<ChangeEvent<Enum>> enumChange;
            EventCallback<ChangeEvent<int>> intChange;
            EventCallback<ChangeEvent<bool>> boolChange;
            EventCallback<ChangeEvent<float>> floatChange;
            EventCallback<ChangeEvent<Color>> colorChange;
            EventCallback<ChangeEvent<List<Vector3>>> listChange;
            EventCallback<ChangeEvent<Object>> objChange;

            #region Path Settings
            VisualElement pathSettings = _inspector.Q<VisualElement>(PathSettingsWrapper);
            int index = 0;

            // Name
            TextField nameField = (TextField)pathSettings.ElementAt(index++);
            nameField.SetValueWithoutNotify(_selectedCurve.PathName);
            nameField.RegisterValueChangedCallback(stringChange = evt =>
            {
                _selectedCurve.PathName = evt.newValue;
                _curves.Remove(evt.previousValue);
                _curves.TryAdd(evt.newValue, _selectedCurve);
                UpdateSelector(evt.newValue);
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<string> { Element = nameField, Event = stringChange });

            // Target Field
            ObjectField targetField = (ObjectField)pathSettings.ElementAt(index++);
            targetField.SetValueWithoutNotify(_selectedCurve.Target);
            targetField.RegisterValueChangedCallback(objChange = evt =>
            {
                if(evt.newValue == null)
                {
                    _selectedCurve.Target = ((PathManager)target).transform;
                    targetField.SetValueWithoutNotify(_selectedCurve.Target);
                }
                else
                {
                    _selectedCurve.Target = (Transform)evt.newValue;
                }
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<Object> { Element = targetField, Event = objChange });

            // Path Type
            EnumField pathType = (EnumField)pathSettings.ElementAt(index++);
            pathType.SetValueWithoutNotify(_selectedCurve.PType);
            pathType.RegisterValueChangedCallback(enumChange = evt =>
            {
                _selectedCurve.PType = (PathType)evt.newValue;
                if (_selectedCurve.PType == PathType.CubicBezier) _selectedCurve.GenerateBezierControls();
                HideCurveSpecificElements();
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<Enum> { Element = pathType, Event = enumChange });

            // Path Space
            EnumField pathSpace = (EnumField)pathSettings.ElementAt(index++);
            pathSpace.SetValueWithoutNotify(_selectedCurve.Space);
            pathSpace.RegisterValueChangedCallback(enumChange = evt =>
            {
                _selectedCurve.Space = (PathSpace)evt.newValue;
                if (_selectedCurve == null) return;
                _selectedCurve.Space = (PathSpace)evt.newValue;
                HideCurveSpecificElements();
                SceneView.RepaintAll();
            });

            // Resolution
            IntegerField resField = (IntegerField)pathSettings.ElementAt(index++);
            resField.SetValueWithoutNotify(_selectedCurve.Resolution);
            resField.RegisterValueChangedCallback(intChange = evt =>
            {
                if (evt.newValue < 1)
                {
                    resField.SetValueWithoutNotify(1);
                    return;
                }
                _selectedCurve.Resolution = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<int> { Element = resField, Event = intChange });

            // Close Path
            Toggle closePath = (Toggle)pathSettings.ElementAt(index++);
            closePath.SetValueWithoutNotify(_selectedCurve.ClosePath);
            closePath.RegisterValueChangedCallback(boolChange = evt =>
            {
                _selectedCurve.ToggleClosePath(evt.newValue);
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<bool> { Element = closePath, Event = boolChange });

            // Start From Current
            Toggle startCurrent = (Toggle)pathSettings.ElementAt(index++);
            startCurrent.SetValueWithoutNotify(_selectedCurve.StartFromCurrent);
            startCurrent.RegisterValueChangedCallback(boolChange = evt =>
            {
                _selectedCurve.ToggleStartFromCurrent(evt.newValue);
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<bool> { Element = startCurrent, Event = boolChange });

            // Generate On Awake
            Toggle genAwake = (Toggle)pathSettings.ElementAt(index++);
            genAwake.SetValueWithoutNotify(_selectedCurve.GenerateOnAwake);
            genAwake.RegisterValueChangedCallback(boolChange = evt =>
            {
                _selectedCurve.GenerateOnAwake = evt.newValue;
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<bool> { Element = genAwake, Event = boolChange });

            // Alpha
            FloatField alphaField = (FloatField)pathSettings.ElementAt(index++);
            alphaField.SetValueWithoutNotify(_selectedCurve.Alpha);
            alphaField.RegisterValueChangedCallback(floatChange = evt =>
            {
                _selectedCurve.Alpha = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<float> { Element = alphaField, Event = floatChange });

            // Waypoints
            _waypoints.SetValueWithoutNotify(_selectedCurve.Waypoints);
            _waypoints.RegisterValueChangedCallback(listChange = evt =>
            {
                _selectedCurve.Waypoints = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<List<Vector3>> { Element = _waypoints, Event = listChange });
            #endregion

            #region Editor Settings
            VisualElement editorSettings = _inspector.Q<VisualElement>(EditorSettingsWrapper);
            index = 0;

            // Draw Indices
            Toggle drawIndices = (Toggle)editorSettings.ElementAt(index++);
            drawIndices.SetValueWithoutNotify(_selectedCurve.DrawIndices);
            drawIndices.RegisterValueChangedCallback(boolChange = evt =>
            {
                _selectedCurve.DrawIndices = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<bool> { Element = drawIndices, Event = boolChange });

            // Move Relative
            Toggle moveRelative = (Toggle)editorSettings.ElementAt(index++);
            moveRelative.SetValueWithoutNotify(_selectedCurve.MoveRelative);
            moveRelative.RegisterValueChangedCallback(boolChange = evt =>
            {
                _selectedCurve.MoveRelative = evt.newValue;
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<bool> { Element = moveRelative, Event = boolChange });

            // Relative Plane
            Toggle relativePlane = (Toggle)editorSettings.ElementAt(index++);
            relativePlane.SetValueWithoutNotify(_selectedCurve.RelativePlane);
            relativePlane.RegisterValueChangedCallback(boolChange = evt =>
            {
                _selectedCurve.RelativePlane = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<bool> { Element = relativePlane, Event = boolChange });

            // Hide Controls
            Toggle hideControls = (Toggle)editorSettings.ElementAt(index++);
            hideControls.SetValueWithoutNotify(_selectedCurve.HideControls);
            hideControls.RegisterValueChangedCallback(boolChange = evt =>
            {
                _selectedCurve.HideControls = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<bool> { Element = hideControls, Event = boolChange });

            // Handle Size
            FloatField handleSize = (FloatField)editorSettings.ElementAt(index++);
            handleSize.SetValueWithoutNotify(_selectedCurve.HandleSize);
            handleSize.RegisterValueChangedCallback(floatChange = evt =>
            {
                if(evt.newValue < 0f)
                {
                    handleSize.SetValueWithoutNotify(0f);
                    return;
                }
                _selectedCurve.HandleSize = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<float> { Element = handleSize, Event = floatChange });

            // Control Type
            EnumField controlType = (EnumField)editorSettings.ElementAt(index++);
            controlType.SetValueWithoutNotify(_selectedCurve.CType);
            controlType.RegisterValueChangedCallback(enumChange = evt =>
            {
                _selectedCurve.CType = (ControlType)evt.newValue;
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<Enum> { Element = controlType, Event = enumChange });

            // Colors
            VisualElement colorsWrapper = editorSettings.Q<VisualElement>(ColorsWrapper);
            index = 0;

            // Path Color
            ColorField pathColor = (ColorField)colorsWrapper.ElementAt(index++);
            pathColor.SetValueWithoutNotify(_selectedCurve.PathColor);
            pathColor.RegisterValueChangedCallback(colorChange = evt =>
            {
                _selectedCurve.PathColor = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<Color> { Element = pathColor, Event = colorChange });

            // Waypoint Color
            ColorField pointColor = (ColorField)colorsWrapper.ElementAt(index++);
            pointColor.SetValueWithoutNotify(_selectedCurve.PointColor);
            pointColor.RegisterValueChangedCallback(colorChange = evt =>
            {
                _selectedCurve.PointColor = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<Color> { Element = pointColor, Event = colorChange });

            // Control Color
            ColorField controlColor = (ColorField)colorsWrapper.ElementAt(index++);
            controlColor.SetValueWithoutNotify(_selectedCurve.ControlColor);
            controlColor.RegisterValueChangedCallback(colorChange = evt =>
            {
                _selectedCurve.ControlColor = evt.newValue;
                SceneView.RepaintAll();
            });
            _eventTokens.Add(new ChangeEventRegistrationToken<Color> { Element = controlColor, Event = colorChange });

            #endregion

        }

        private void UpdateSelector(string text = null)
        {
            ToolbarMenu selector = (ToolbarMenu)_inspector.Q<VisualElement>(ManagementWrapper).ElementAt(0);
            if (text != null) selector.text = text;
            selector.ClearMenu();
            selector.menu.AppendAction("NONE", a =>
            {
                _selectedCurve = null;
                SetValue(SelectedCurveProp, null);
                selector.text = "NONE";
                PopulateInspector();
            });
            foreach (PathCurve curve in _curves.Values)
            {
                if (curve == null) continue;
                selector.menu.AppendAction(curve.PathName, a =>
                {
                    _selectedCurve = curve;
                    SetValue(SelectedCurveProp, _selectedCurve);
                    selector.text = curve.PathName;
                    PopulateInspector();
                });
            }
        }
        #endregion

        private void SetValue(string property, object value)
        {
            serializedObject.FindProperty(property).SetValue(value);
        }

        private void AddWaypoint()
        {
            float depthFor3DSpace = 10f;
            Ray mouseRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            Vector3 worldMouse = Physics.Raycast(mouseRay, out var hitInfo, depthFor3DSpace * 2f) ?
                hitInfo.point : mouseRay.GetPoint(depthFor3DSpace);
            if (_selectedCurve == null) return;
            _selectedCurve.AddWaypoint(worldMouse);
            _waypoints.SetValueWithoutNotify(_selectedCurve.Waypoints);
        }

        private void MouseOverPoint()
        {
            if (_selectedCurve == null) return;
            Vector2 mousePos = Event.current.mousePosition;
            for(int i = 0; i < _selectedCurve.Waypoints.Count; i++)
            {
                Vector2 point = HandleUtility.WorldToGUIPoint(_selectedCurve.Waypoints[i]);
                if (IsOver(point))
                {
                    _hoverPointIndex = i;
                    SceneView.RepaintAll();
                    return;
                }
            }
            for(int i = 0; i < _selectedCurve.BezierControls.Count; i++)
            {
                Vector2 point = HandleUtility.WorldToGUIPoint(_selectedCurve.BezierControls[i]);
                if (IsOver(point))
                {
                    _hoverPointIndex = i + _selectedCurve.Waypoints.Count;
                    SceneView.RepaintAll();
                    return;
                }
            }
            _hoverPointIndex = -1;
            SceneView.RepaintAll();

            bool IsOver(Vector2 point)
            {
                float delta = (mousePos - point).magnitude;
                float error = HandleUtility.GetHandleSize(point) * PathCurve.MouseOverError;
                return (delta < error);
            }
        }

        private void HideCurveSpecificElements()
        {
            if (_selectedCurve == null) return;
            VisualElement pathSettings = _inspector.Q<VisualElement>(PathSettingsWrapper);

            // Alpha
            VisualElement alpha = pathSettings.ElementAt(7);
            alpha.style.height = _selectedCurve.PType == PathType.CatmullRom ?
                new StyleLength(StyleKeyword.Auto) :
                new StyleLength(0f);

            VisualElement editorSettings = _inspector.Q<VisualElement>(EditorSettingsWrapper);

            // Relative Plane
            VisualElement relativePlane = editorSettings.ElementAt(2);
            relativePlane.style.height = _selectedCurve.Space != PathSpace.Full3D ?
                new StyleLength(StyleKeyword.Auto) :
                new StyleLength(0f);

            // Hide Controls
            VisualElement hideControls = editorSettings.ElementAt(3);
            hideControls.style.height = _selectedCurve.PType == PathType.CubicBezier ?
                new StyleLength(StyleKeyword.Auto) :
                new StyleLength(0f);

            // Control Type
            VisualElement controlType = editorSettings.ElementAt(5);
            controlType.style.height = _selectedCurve.PType == PathType.CubicBezier ?
                new StyleLength(StyleKeyword.Auto) :
                new StyleLength(0f);
        }

        #region DON'T DELETE
        void MoveWaypoints(Vector3 delta)
        {
            for (int i = 0; i < _selectedCurve.Waypoints.Count; i++)
            {
                _selectedCurve.Waypoints[i] += delta;
                _waypoints.SetElementWithoutNotify(i, _selectedCurve.Waypoints[i]);
            }
            for (int i = 0; i < _selectedCurve.BezierControls.Count; i++)
            {
                _selectedCurve.BezierControls[i] += delta;
            }
        }
        void RotateWaypoints(Quaternion delta)
        {
            for (int i = 0; i < _selectedCurve.Waypoints.Count; i++)
            {
                Vector3 point = _selectedCurve.Waypoints[i];
                Quaternion pointRot = new Quaternion(point.x, point.y, point.z, 0);
                Quaternion rotatedPoint = delta * pointRot * Quaternion.Inverse(delta);
                _selectedCurve.Waypoints[i] = new Vector3(rotatedPoint.x, rotatedPoint.y, rotatedPoint.z);
                _waypoints.SetElementWithoutNotify(i, _selectedCurve.Waypoints[i]);
            }
            for (int i = 0; i < _selectedCurve.BezierControls.Count; i++)
            {
                Vector3 point = _selectedCurve.BezierControls[i];
                Quaternion pointRot = new Quaternion(point.x, point.y, point.z, 0);
                Quaternion rotatedPoint = delta * pointRot * Quaternion.Inverse(delta);
                _selectedCurve.BezierControls[i] = new Vector3(rotatedPoint.x, rotatedPoint.y, rotatedPoint.z);
            }
        }
        void ScaleWaypoints(Vector3 delta)
        {
            Vector3 targetPos = ((PathManager)target).transform.position;

            for (int i = 0; i < _selectedCurve.Waypoints.Count; i++)
            {
                Vector3 normal = (_selectedCurve.Waypoints[i] - targetPos).normalized;
                Vector3 disp = new Vector3(
                    normal.x * delta.x,
                    normal.y * delta.y,
                    normal.z * delta.z
                );
                _selectedCurve.Waypoints[i] += disp;
                _waypoints.SetElementWithoutNotify(i, _selectedCurve.Waypoints[i]);
            }
            for (int i = 0; i < _selectedCurve.BezierControls.Count; i++)
            {
                Vector3 normal = (_selectedCurve.BezierControls[i] - targetPos).normalized;
                Vector3 disp = new Vector3(
                    normal.x * delta.x,
                    normal.y * delta.y,
                    normal.z * delta.z
                );
                _selectedCurve.BezierControls[i] += disp;
            }
        }
        void DrawWaypoints()
        {
            Color temp = Handles.color;
            Handles.color = _selectedCurve.PointColor;
            for (int i = 0; i < _selectedCurve.Waypoints.Count; i++)
            {
                // Draw the point
                Vector3 point = Handles.FreeMoveHandle(
                    _selectedCurve.Waypoints[i],
                    Quaternion.identity,
                    _selectedCurve.HandleSize,
                    PathCurve.Snap,
                    Handles.SphereHandleCap
                );
                if(_drawPosHandle && i == _selectedPointIndex)
                {
                    point = Handles.PositionHandle(
                        _selectedCurve.Waypoints[i],
                        Quaternion.identity
                    );
                }
                else if(point - _selectedCurve.Waypoints[i] != Vector3.zero)
                {
                    // If the point was moved but without the pos handle then deselect it
                    _selectedPointIndex = -1;
                }

                // Confine the point to the path space
                Vector3 space = _selectedCurve.RelativePlane ?
                    _selectedCurve.Target.position :
                    Vector3.zero;
                if (_selectedCurve.Space == PathSpace.XY) point.z = space.z;
                else if (_selectedCurve.Space == PathSpace.XZ) point.y = space.y;
                else if (_selectedCurve.Space == PathSpace.YZ) point.x = space.x;

                // Draw the indices if needed
                if (_selectedCurve.DrawIndices)
                {
                    Handles.Label(point + Vector3.one * _selectedCurve.HandleSize, i.ToString());
                }

                // Draw hover and select rings
                if (i == _selectedPointIndex || i == _hoverPointIndex)
                {
                    Handles.color = i == _selectedPointIndex ? PathCurve.PointSelectColor : PathCurve.PointHoverColor;
                    Handles.CircleHandleCap(
                        0,
                        _selectedCurve.Waypoints[i],
                        SceneView.currentDrawingSceneView.rotation.normalized,
                        _selectedCurve.HandleSize,
                        EventType.Repaint
                    );
                    Handles.color = _selectedCurve.PointColor;
                }

                _selectedCurve.Waypoints[i] = point;
                _waypoints.SetElementWithoutNotify(i, point);
            }
            Handles.color = temp;
        }
        void DrawCBControls()
        {
            if (_selectedCurve.PType != PathType.CubicBezier || _selectedCurve.HideControls) return;
            int pointIndex = 0;
            List<Vector3> temp = new List<Vector3>(_selectedCurve.Waypoints);
            if (_selectedCurve.StartFromCurrent) temp.Insert(0, ((PathManager)target).transform.position);
            if (_selectedCurve.ClosePath) temp.Add(temp[0]);

            Color c = Handles.color;
            Handles.color = _selectedCurve.ControlColor;
            float size = _selectedCurve.HandleSize * PathCurve.ControlSizeRatio;
            int selectedIndex = _selectedPointIndex - _selectedCurve.Waypoints.Count;
            int hoverIndex = _hoverPointIndex - _selectedCurve.Waypoints.Count;
            for (int i = 0; i < _selectedCurve.BezierControls.Count; i++)
            {
                Handles.DrawDottedLine(_selectedCurve.BezierControls[i], temp[pointIndex], 5f);

                if (i == selectedIndex || i == hoverIndex)
                {
                    Handles.color = i == selectedIndex ? PathCurve.PointSelectColor : PathCurve.PointHoverColor;
                    float handleSize = HandleUtility.GetHandleSize(_selectedCurve.BezierControls[i]);
                    Handles.CircleHandleCap(
                        0,
                        _selectedCurve.BezierControls[i],
                        SceneView.currentDrawingSceneView.rotation.normalized,
                        size,
                        EventType.Repaint
                    );
                    Handles.color = _selectedCurve.ControlColor;
                }
                Vector3 point = Handles.FreeMoveHandle(
                    _selectedCurve.BezierControls[i],
                    Quaternion.identity,
                    size,
                    PathCurve.Snap,
                    Handles.SphereHandleCap
                );
                if (_drawPosHandle && i == selectedIndex)
                {
                    point = Handles.PositionHandle(
                        _selectedCurve.BezierControls[i],
                        Quaternion.identity
                    );
                }

                Vector3 space = _selectedCurve.RelativePlane ?
                    _selectedCurve.Target.position :
                    Vector3.zero;
                if (_selectedCurve.Space == PathSpace.XY) point.z = space.z;
                else if (_selectedCurve.Space == PathSpace.XZ) point.y = space.y;
                else if (_selectedCurve.Space == PathSpace.YZ) point.x = space.x;

                Vector3 delta = point - _selectedCurve.BezierControls[i];
                int partnerIndex;
                if (_selectedCurve.ClosePath && i == 0)
                {
                    partnerIndex = _selectedCurve.BezierControls.Count - 1;
                }
                else if(_selectedCurve.ClosePath && i == _selectedCurve.BezierControls.Count - 1)
                {
                    partnerIndex = 0;
                }
                else
                {
                    partnerIndex = i % 2 == 0 ? i - 1 : i + 1;
                }

                if (delta != Vector3.zero && _selectedCurve.CType != ControlType.Free &&
                    partnerIndex >= 0 && partnerIndex < _selectedCurve.BezierControls.Count)
                {
                    // Move partner control
                    int waypointIndex = i % 2 == 0 ? i / 2 : (i + 1) / 2;
                    if (_selectedCurve.ClosePath && i == _selectedCurve.BezierControls.Count - 1) waypointIndex = 0;

                    Vector3 waypoint = _selectedCurve.Waypoints[waypointIndex];
                    Vector3 normal = waypoint - point;

                    if (_selectedCurve.CType == ControlType.Aligned)
                    {
                        Vector3 unitNormal = normal.normalized;
                        float partnerDist = (waypoint - _selectedCurve.BezierControls[partnerIndex]).magnitude;
                        _selectedCurve.BezierControls[partnerIndex] = (unitNormal * partnerDist) + waypoint;
                    }
                    else
                    {
                        _selectedCurve.BezierControls[partnerIndex] = normal + waypoint;
                    }
                }

                _selectedCurve.BezierControls[i] = point;
                if (i % 2 == 0) pointIndex++;
            }
            Handles.color = c;
        }
        void DrawArrows()
        {
            if (_selectedCurve.Waypoints.Count == 0) return;
            Color temp = Handles.color;
            Handles.color = new Color(
                _selectedCurve.PointColor.r,
                _selectedCurve.PointColor.g,
                _selectedCurve.PointColor.b,
                _selectedCurve.PointColor.a * PathCurve.ArrowFadeRatio
            );
            for (int i = 0; i < _selectedCurve.Waypoints.Count - 1; i++)
            {
                DrawArrow(_selectedCurve.Waypoints[i], _selectedCurve.Waypoints[i + 1]);
            }
            Vector3 start = _selectedCurve.Waypoints[0];
            if (_selectedCurve.StartFromCurrent)
            {
                start = ((PathManager)target).transform.position;
                DrawArrow(start, _selectedCurve.Waypoints[0]);
            }
            if (_selectedCurve.ClosePath)
            {
                DrawArrow(_selectedCurve.Waypoints[_selectedCurve.Waypoints.Count - 1], start);
            }

            Handles.color = temp;

            void DrawArrow(Vector3 p1, Vector3 p2)
            {
                Vector3 normal = (p2 - p1).normalized;
                if (normal != Vector3.zero)
                {
                    Vector3 pos = p1 + _selectedCurve.HandleSize * PathCurve.ArrowOffsetRatio * normal;
                    Handles.ConeHandleCap(
                        0,
                        pos,
                        Quaternion.LookRotation(normal),
                        _selectedCurve.HandleSize * PathCurve.ArrowSizeRatio,
                        EventType.Repaint
                    );
                }
            }
        }
        void DrawCurve()
        {
            Color temp = Handles.color;
            Handles.color = _selectedCurve.PathColor;
            float thickness = _selectedCurve.HandleSize * PathCurve.CurveThicknessRatio;
            Vector3[] points = _selectedCurve.WaypointsToCurve();
            for (int i = 0; i < points.Length - 1; i++)
            {
                Handles.DrawLine(points[i], points[i + 1], thickness);
            }
            Handles.color = temp;
        }
        private void SetInitialTransform()
        {
            Transform transform = ((PathManager)target).transform;
            _targetPos = transform.position;
            _targetScale = transform.localScale;
            _targetRot = transform.rotation;
        }
        #endregion
    }
}
