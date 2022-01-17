using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DG.Tweening;

namespace DT_Animation
{
    internal class DTController
    {
        public DTSequencer sequencer;
        public DTAnimator animator;

        private List<SequenceController> sequenceControllers;
        private SequencerData activeData;

        public DTController()
        {
            sequenceControllers = new List<SequenceController>();
        }

        public DTController(DTAnimator animator) : this()
        {
            this.animator = animator;
            activeData = animator.data;
        }

        /// <param name="name">Name of Asset to find. NO EXTENSIONS.</param>
        /// <returns>The path to the Asset found or null</returns>
        private string FindData(string name)
        {
            string[] files = Directory.GetFiles(SequencerVals.sequencerLibrary);
            foreach (string file in files)
            {
                if (name == Path.GetFileNameWithoutExtension(file))
                {
                    return file;
                }
            }
            return null;
        }

        #region Header Commands
        /// <summary>
        /// Clears the sequencer
        /// </summary>
        public void ClearSequencer()
        {
            Debug.Log("Clearing Sequencer");
            activeData = null;
            animator = null;
            sequenceControllers.Clear();
            sequencer.Clear();
        }

        /// <summary>
        /// Saves sequencer data to the sequencer library
        /// </summary>
        public void SaveSequencer()
        {
            if (!sequencer.nameField.ClassListContains(SequencerVals.nameFieldPlaceHolderClass))
            {
                string sequencerName = sequencer.nameField.value;
                string foundAsset = FindData(sequencerName);
                if (activeData == null && foundAsset == null && !string.IsNullOrEmpty(sequencerName))
                {
                    // Save New
                    Debug.Log($"Saving New Asset: {sequencerName}");
                    activeData = ScriptableObject.CreateInstance<SequencerData>();
                    activeData.name = sequencerName;
                    activeData.sequences = ControllersToArray();
                    AssetDatabase.CreateAsset(activeData, $"{SequencerVals.sequencerLibrary}/{activeData.name}.asset");
                    AssetDatabase.SaveAssets();
                }
                else if (activeData.name == sequencerName && sequencerName == Path.GetFileNameWithoutExtension(foundAsset))
                {
                    // Save Update
                    Debug.Log($"Saving Update: {sequencerName}");
                    activeData.sequences = ControllersToArray();
                    AssetDatabase.SaveAssets();
                }
                else if (activeData.name != sequencerName && foundAsset == null)
                {
                    // Save Rename
                    Debug.Log($"Saving Update and Renaming: {activeData.name} to {sequencerName}");
                    AssetDatabase.RenameAsset(
                        $"{SequencerVals.sequencerLibrary}/{activeData.name}.asset",
                        sequencerName
                    );
                    activeData.name = sequencerName;
                    activeData.sequences = ControllersToArray();
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    // Already Exists
                    Debug.LogWarning("Asset Already Exists!");
                }

                if(animator != null)
                {
                    animator.data = activeData;
                    EditorUtility.SetDirty(animator);
                }
            }
            else
            {
                Debug.LogWarning("Name the Sequencer!");
            }
        }

        /// <summary>
        /// Condenses the sequence controllers list into an array of DTSequences
        /// </summary>
        /// <returns>The new DTSequence array</returns>
        private DTSequence[] ControllersToArray()
        {
            DTSequence[] sequences = new DTSequence[sequenceControllers.Count];

            int count = 0;
            foreach(SequenceController sequenceController in sequenceControllers)
            {
                sequenceController.sequence.tweens = new List<DTTween>();
                foreach(TweenController tweenController in sequenceController.tweenControllers)
                {
                    sequenceController.sequence.tweens.Add(tweenController.tween);
                }
                sequences[count++] = sequenceController.sequence;
            }

            return sequences;
        }

        /// <summary>
        /// Loads sequencer data by the name field from the sequencer library
        /// </summary>
        public void LoadSequencer()
        {
            if (!sequencer.nameField.ClassListContains(SequencerVals.nameFieldPlaceHolderClass))
            {
                string sequencerName = sequencer.nameField.value;
                string foundAsset = FindData(sequencer.nameField.value);
                if (foundAsset == null)
                {
                    Debug.LogWarning($"Unable to Find Asset: {sequencerName}");
                }
                else
                {
                    Debug.Log($"Loading Asset: {sequencerName}");
                    activeData = AssetDatabase.LoadAssetAtPath<SequencerData>($"{SequencerVals.sequencerLibrary}/{sequencerName}.asset");
                    sequenceControllers.Clear();
                    LoadActiveData();                   
                }
            }
            else
            {
                Debug.LogWarning("Unable to Find Asset! Write Desired Asset in Name Field");
            }
        }

        public void LoadActiveData()
        {
            // Generate the controllers
            foreach (DTSequence sequence in activeData.sequences)
            {
                SequenceController sequenceController = new SequenceController()
                {
                    sequence = sequence,
                    tweenControllers = new List<TweenController>()
                };
                foreach (DTTween tween in sequence.tweens)
                {
                    TweenController tweenController = new TweenController()
                    {
                        tween = tween,
                        sequenceController = sequenceController
                    };
                    sequenceController.tweenControllers.Add(tweenController);
                }

                sequenceControllers.Add(sequenceController);
            }
            sequencer.GenerateView(sequenceControllers);
            sequencer.nameField.SetText(activeData.name);
        }
        
        /// <summary>
        /// Delete sequencer data and clear the sequencer
        /// </summary>
        public void DeleteSequencer()
        {
            if (!sequencer.nameField.ClassListContains(SequencerVals.nameFieldPlaceHolderClass))
            {
                string asset = FindData(sequencer.nameField.value);
                if (asset == null)
                {
                    Debug.LogWarning($"Unable to Find Asset {sequencer.nameField.value}");
                }
                else
                {
                    Debug.Log($"Deleting Asset: {sequencer.nameField.value}");
                    activeData = null;
                    sequenceControllers.Clear();
                    sequencer.Clear();
                    sequencer.nameField.ClearToPlaceHolder(SequencerVals.sequencerNamePlaceholder);
                    AssetDatabase.DeleteAsset(asset);
                }

                if(animator != null)
                {
                    animator.data = null;
                }
            }
            else
            {
                Debug.LogWarning("Unable to Find Asset! Write Desired Sequencer in Name Field");
            }
        }
        #endregion

        /// <summary>
        /// Creates a new sequence.
        /// </summary>
        public void CreateSequence()
        {
            SequenceController controller = new SequenceController()
            {
                sequence = new DTSequence(),
                tweenControllers = new List<TweenController>()
            };
            sequencer.CreateSequence(controller);
            sequenceControllers.Add(controller);
        }
       
        /// <summary>
        /// Deletes an existing sequence.
        /// </summary>
        /// <param name="sequenceController">The sequence to delete.</param>
        public void DeleteSequence(SequenceController sequenceController)
        {
            if(sequenceController.joiner != null)
            {
                if (sequenceController.isJoined)
                {
                    JoinSequence(false, sequenceController);
                    if(sequenceController.joiner != null)
                    {
                        SequenceController partner = sequenceControllers[sequenceControllers.IndexOf(sequenceController) + 1];
                        JoinSequence(false, partner);
                        Toggle joinToggle = partner.visualElement.SearchChildren<Toggle>(SequencerVals.joinToggleName);
                        joinToggle.SetValueWithoutNotify(false);
                    }
                }
                else
                {
                    JoinSequence(false, sequenceControllers[sequenceControllers.IndexOf(sequenceController) + 1]);
                }
            }
            if(sequenceControllers.IndexOf(sequenceController) == 0)
            {
                sequencer.ToggleJoinToggleVisibility(sequenceControllers[1].visualElement);
            }
            sequencer.tweenContainer.Remove(sequenceController.tweenList);
            sequenceControllers.Remove(sequenceController);
            sequencer.sequenceContainer.Remove(sequenceController.visualElement);
        }

        public void DeleteTween(SequenceController sequenceController, TweenController tweenController)
        {
            if(tweenController.joiner != null)
            {
                if (tweenController.isJoined)
                {
                    JoinTween(false, tweenController);
                    if(tweenController.joiner != null)
                    {
                        TweenController partner = sequenceController.tweenControllers[sequenceController.tweenControllers.IndexOf(tweenController) + 1];
                        JoinTween(false, partner);
                        Toggle joinToggle = partner.visualElement.SearchChildren<Toggle>(SequencerVals.joinToggleName);
                        joinToggle.SetValueWithoutNotify(false);
                    }
                }
                else
                {
                    JoinTween(false, sequenceController.tweenControllers[sequenceController.tweenControllers.IndexOf(tweenController) + 1]);
                }
            }
            if (sequenceController.tweenControllers.IndexOf(tweenController) == 0)
            {
                sequencer.ToggleJoinToggleVisibility(sequenceController.tweenControllers[1].visualElement);
            }
            sequenceController.tweenControllers.Remove(tweenController);
            tweenController.sequenceController.tweenList.Remove(tweenController.visualElement);
        }

        /// <summary>
        /// Creates a new tween.
        /// </summary>
        /// <param name="controller">The sequence this tween will be attached to.</param>
        public void CreateTween(SequenceController controller)
        {
            sequencer.CreateTween(controller);
        }

        /// <summary>
        /// Joins this sequence with the previous sequence in the list.
        /// </summary>
        /// <param name="evt">The input event.</param>
        /// <param name="sequenceController">The sequence attempting to join with the pevious sequence.</param>
        public void JoinSequence(bool isJoined, SequenceController sequenceController)
        {
            sequenceController.isJoined = isJoined;
            sequenceController.sequence.isJoined = isJoined;
            List<TwequenceController> twequences = new List<TwequenceController>(sequenceControllers);
            JoinTwequence(sequenceController, twequences, SequencerVals.sequenceJoiner, sequencer.sequenceContainer);
        }

        /// <summary>
        /// Joins this tween with the previous tween in the list.
        /// </summary>
        /// <param name="evt">The input event.</param>
        /// <param name="tweenController">The twen attempting to join with the previous tseen.</param>
        public void JoinTween(bool isJoined, TweenController tweenController)
        {
            tweenController.isJoined = isJoined;
            tweenController.tween.isJoined = isJoined;
            SequenceController sequenceController = tweenController.sequenceController;
            List<TwequenceController> twequences = new List<TwequenceController>(sequenceController.tweenControllers);
            JoinTwequence(tweenController, twequences, SequencerVals.tweenJoiner, sequenceController.tweenList);
        }
        
        /// <summary>
        /// A helper function to join tweens and sequences.
        /// </summary>
        /// <param name="twequenceController">The tween or sequence attempting to make a join.</param>
        /// <param name="controllers">The list of controllers this twequence belongs to.</param>
        /// <param name="joinerClass">The uss class for the joiner element.</param>
        /// <param name="container">The visual element which holds the twequence visual elements.</param>
        private void JoinTwequence(TwequenceController twequenceController, List<TwequenceController> controllers, 
            string joinerClass, VisualElement container)
        {
            int controllerIndex = controllers.FindIndex(t => t == twequenceController);
            if (twequenceController.isJoined)
            {
                TwequenceController partner = controllers[controllerIndex - 1];
                bool myJoiner = twequenceController.visualElement.parent.ClassListContains(joinerClass);
                bool partnerJoiner = partner.visualElement.parent.ClassListContains(joinerClass);
                if (myJoiner)
                {
                    twequenceController.joiner.Insert(0, partner.visualElement);
                    if (partner.joiner != null)
                    {
                        // Loop backwards collecting new partners
                        for (int i = controllerIndex - 2; i > -1; i--)
                        {
                            TwequenceController tc = controllers[i];
                            if (tc.joiner != partner.joiner)
                            {
                                break;
                            }
                            twequenceController.joiner.Insert(0, tc.visualElement);
                            tc.joiner = twequenceController.joiner;
                        }
                    }
                    if (partner.joiner != null)
                    {
                        container.Remove(partner.joiner);
                    }
                    partner.joiner = twequenceController.joiner;
                }
                else if (partnerJoiner)
                {
                    partner.joiner.Add(twequenceController.visualElement);
                    if (twequenceController.joiner != null)
                    {
                        // Loop forwards collecting new partners
                        for (int i = controllerIndex + 1; i < sequenceControllers.Count; i++)
                        {
                            TwequenceController tc = controllers[i];
                            if (tc.joiner != twequenceController.joiner)
                            {
                                break;
                            }
                            partner.joiner.Add(tc.visualElement);
                            tc.joiner = partner.joiner;
                        }
                        container.Remove(twequenceController.joiner);
                    }
                    twequenceController.joiner = partner.joiner;
                }
                else
                {
                    // Replace the visual element with a joiner, then parent the visual element to the joiner
                    VisualElement joiner = new VisualElement();
                    joiner.AddToClassList(joinerClass);
                    int viewIndex = container.IndexOf(twequenceController.visualElement);

                    container.RemoveAt(viewIndex);
                    container.Insert(viewIndex, joiner);
                    container.RemoveAt(viewIndex - 1);

                    twequenceController.joiner = joiner;
                    partner.joiner = joiner;
                    joiner.Add(partner.visualElement);
                    joiner.Add(twequenceController.visualElement);
                }
            }
            else
            {
                VisualElement joiner = twequenceController.joiner;
                int joinerIndex = container.IndexOf(joiner);

                TwequenceController leftNeighbor = controllerIndex - 1 > -1 ?
                    controllers[controllerIndex - 1] :
                    null;
                TwequenceController rightNeighbor = controllerIndex + 1 < controllers.Count ?
                    controllers[controllerIndex + 1] :
                    null;
                bool leftJoined = leftNeighbor != null && leftNeighbor.isJoined;
                bool rightJoined = rightNeighbor != null && rightNeighbor.isJoined;

                if (leftJoined && rightJoined)
                {
                    // Split the Joiner. Put everything to the left of me in a new joiner.
                    VisualElement splitJoiner = new VisualElement();
                    splitJoiner.AddToClassList(joinerClass);
                    int index = controllerIndex - 1;
                    TwequenceController curController = controllers[index];
                    while (curController.joiner == joiner)
                    {
                        joiner.Remove(curController.visualElement);
                        splitJoiner.Insert(0, curController.visualElement);
                        curController.joiner = splitJoiner;
                        if (--index < 0)
                        {
                            break;
                        }
                        curController = controllers[index];
                    }
                    container.Insert(container.IndexOf(joiner), splitJoiner);
                }
                else
                {
                    if (!leftJoined && leftNeighbor != null)
                    {
                        joiner.Remove(leftNeighbor.visualElement);
                        leftNeighbor.joiner = null;
                        container.Insert(joinerIndex, leftNeighbor.visualElement);
                    }
                    if (!rightJoined && rightNeighbor != null)
                    {
                        joiner.Remove(twequenceController.visualElement);
                        twequenceController.joiner = null;
                        container.Insert(joinerIndex + 1, twequenceController.visualElement);
                    }
                    if (controllerIndex == controllers.Count - 1)
                    {
                        joiner.Remove(twequenceController.visualElement);
                        twequenceController.joiner = null;
                        container.Insert(joinerIndex + 1, twequenceController.visualElement);
                    }

                    if (joiner.childCount == 0)
                    {
                        container.Remove(joiner);
                    }
                }
            }
        }
    
        public void DemoSequence(string name)
        {
            //if (animator != null)
            //{
            //    Sequence s = animator.GenerateSequence(sequenceControllers.Find(s => s.sequence.name == name).sequence);
            //    s.SetAutoKill(true);
            //}
        }
    }

    internal class TwequenceController
    {
        public VisualElement visualElement, joiner;
        public bool isJoined;
    }

    internal class SequenceController : TwequenceController
    {
        public VisualElement tweenList;
        public DTSequence sequence;
        public List<TweenController> tweenControllers;

        public void OnNameChanged(ChangeEvent<string> evt)
        {
            sequence.name = evt.newValue;
        }

        public void OnDelayChanged(ChangeEvent<float> evt)
        {
            sequence.delay = evt.newValue;
        }

        public void OnLoopsChanged(ChangeEvent<int> evt)
        {
            sequence.loops = evt.newValue;
        }
    }

    internal class TweenController : TwequenceController
    {
        public DTTween tween;
        public SequenceController sequenceController;

        public void AddCurveField()
        {
            CurveField curveField = new CurveField();
            visualElement.Add(curveField);
            curveField.SetValueWithoutNotify(tween.customCurve);
            curveField.RegisterValueChangedCallback(OnCurveChanged);
        }

        public void SetStartEndField(TweenType type)
        {
            VisualElement startEnd = ((List<VisualElement>)visualElement.Children())
                .Find(e => e.ClassListContains(SequencerVals.startEndField));
            VisualElement startGroup = new VisualElement();
            VisualElement endGroup = new VisualElement();
            VisualElement startField, endField;
            startEnd.Clear();

            switch (type)
            {
                case TweenType.Move:
                case TweenType.LocalMove:
                case TweenType.Rotate:
                case TweenType.LocalRotate:
                case TweenType.Scale:
                    tween.isFloat = false;

                    if(tween.startVec == null)
                    {
                        tween.startVec = new Vector3();
                    }
                    if(tween.endVec == null)
                    {
                        tween.endVec = new Vector3();
                    }

                    startEnd.style.flexDirection = FlexDirection.Column;
                    startGroup.style.width = new StyleLength(new Length(100, LengthUnit.Percent));
                    endGroup.style.width = new StyleLength(new Length(100, LengthUnit.Percent));

                    #region Start Field
                    startField = new Vector3Field();
                    FloatField field = startField.SearchChildren<FloatField>(SequencerVals.vecXField);
                    field.RegisterValueChangedCallback(evt => OnStartChanged(evt, 'x'));
                    field.SetValueWithoutNotify(tween.startVec.x);

                    field = startField.SearchChildren<FloatField>(SequencerVals.vecYField);
                    field.RegisterValueChangedCallback(evt => OnStartChanged(evt, 'y'));
                    field.SetValueWithoutNotify(tween.startVec.y);

                    field = startField.SearchChildren<FloatField>(SequencerVals.vecZField);
                    field.RegisterValueChangedCallback(evt => OnStartChanged(evt, 'z'));
                    field.SetValueWithoutNotify(tween.startVec.z);
                    #endregion

                    #region End Field
                    endField = new Vector3Field();
                    field = endField.SearchChildren<FloatField>(SequencerVals.vecXField);
                    field.RegisterValueChangedCallback(evt => OnEndChanged(evt, 'x'));
                    field.SetValueWithoutNotify(tween.endVec.x);

                    field = endField.SearchChildren<FloatField>(SequencerVals.vecYField);
                    field.RegisterValueChangedCallback(evt => OnEndChanged(evt, 'y'));
                    field.SetValueWithoutNotify(tween.endVec.y);

                    field = endField.SearchChildren<FloatField>(SequencerVals.vecZField);
                    field.RegisterValueChangedCallback(evt => OnEndChanged(evt, 'z'));
                    field.SetValueWithoutNotify(tween.endVec.z);
                    #endregion

                    break;
                case TweenType.Fade:
                    tween.isFloat = true;

                    startEnd.style.flexDirection = FlexDirection.Row;
                    startGroup.style.width = new StyleLength(new Length(50, LengthUnit.Percent));
                    endGroup.style.width = new StyleLength(new Length(50, LengthUnit.Percent));

                    startField = new FloatField();
                    ((FloatField)startField).RegisterValueChangedCallback(OnStartChanged);
                    ((FloatField)startField).SetValueWithoutNotify(tween.startVal);
                    endField = new FloatField();
                    ((FloatField)endField).RegisterValueChangedCallback(OnEndChanged);
                    ((FloatField)endField).SetValueWithoutNotify(tween.endVal);
                    break;
                default:
                    startField = null;
                    endField = null;
                    break;
            }
            if (startField != null)
            {
                Label startLabel = new Label("Start");
                startLabel.AddToClassList(SequencerVals.startEndLabel);
                Label endLabel = new Label("End");
                endLabel.AddToClassList(SequencerVals.startEndLabel);

                startGroup.Add(startLabel);
                startGroup.Add(startField);
                endGroup.Add(endLabel);
                endGroup.Add(endField);

                startEnd.Add(startGroup);
                startEnd.Add(endGroup);
            }
        }

        public void OnStartChanged(ChangeEvent<float> evt, char field)
        {
            switch (field)
            {
                case 'x':
                    tween.startVec.x = evt.newValue;
                    break;
                case 'y':
                    tween.startVec.y = evt.newValue;
                    break;
                case 'z':
                    tween.startVec.z = evt.newValue;
                    break;
            }
        }

        public void OnStartChanged(ChangeEvent<float> evt)
        {
            tween.startVal = evt.newValue;
        }

        public void OnEndChanged(ChangeEvent<float> evt, char field)
        {
            switch (field)
            {
                case 'x':
                    tween.endVec.x = evt.newValue;
                    break;
                case 'y':
                    tween.endVec.y = evt.newValue;
                    break;
                case 'z':
                    tween.endVec.z = evt.newValue;
                    break;
            }
        }

        public void OnEndChanged(ChangeEvent<float> evt)
        {
            tween.endVal = evt.newValue;
        }

        public void OnTweenTypeChanged(ChangeEvent<System.Enum> evt)
        {
            tween.tweenType = (TweenType)evt.newValue;
            SetStartEndField(tween.tweenType);
        }

        public void OnDurationChanged(ChangeEvent<float> evt)
        {
            tween.duration = evt.newValue;
        }

        public void OnDelayChanged(ChangeEvent<float> evt)
        {
            tween.delay = evt.newValue;
        }

        public void OnEaseChanged(ChangeEvent<System.Enum> evt)
        {
            tween.ease = (Ease)evt.newValue;
            if (tween.ease == Ease.INTERNAL_Custom)
            {
                AddCurveField();
            }
            else if((Ease)evt.previousValue == Ease.INTERNAL_Custom)
            {
                // try to remove the custom curve
                CurveField curveField = null;
                foreach(VisualElement element in visualElement.Children())
                {
                    if(element is CurveField field)
                    {
                        curveField = field;
                        break;
                    }
                }
                if (curveField != null)
                {
                    try
                    {
                        visualElement.Remove(curveField);
                    }
                    catch (System.ArgumentException) { }
                }
            }
        }
    
        public void OnCurveChanged(ChangeEvent<AnimationCurve> evt)
        {
            tween.customCurve = evt.newValue;
        }
    
        public void OnLoopsChanged(ChangeEvent<int> evt)
        {
            tween.loops = evt.newValue;
        }
    }
}
