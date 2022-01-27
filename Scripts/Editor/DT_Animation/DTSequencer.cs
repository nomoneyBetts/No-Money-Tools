using System.Collections.Generic;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using DG.Tweening;
using UnityEngine;

namespace DT_Animation
{
    public class DTSequencer : EditorWindow
    {
        private static DTController controller;

        public TextField nameField
        {
            get; private set;
        }

        public ScrollView sequenceContainer { get; private set; }
        public ScrollView tweenContainer{ get; private set; }
        private VisualElement header, sequencerBody;

        [MenuItem("Window/Sequencing/DT Sequencer")]
        public static void ShowWindow()
        {
            controller = new DTController();
            DTSequencer sequencer = GetWindow<DTSequencer>("DT Sequencer");
            controller.sequencer = sequencer;
        }

        public static void ShowWindow(DTAnimator animator)
        {
            controller = new DTController(animator);
            DTSequencer sequencer = GetWindow<DTSequencer>("DT Sequencer");
            controller.sequencer = sequencer;
            if (animator.data != null)
            {
                controller.LoadActiveData();
            }
        }

        private void CreateGUI()
        {
            StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(
                LibrariesAccessor.noMoneyRoot + "\\Scripts\\Editor\\DT_Animation\\DTSequencer.uss"
            );
            rootVisualElement.styleSheets.Add(styleSheet);

            header = CreateHeader();
            sequencerBody = CreateSequencerBody();
            rootVisualElement.Add(header);
            rootVisualElement.Add(sequencerBody);
        }

        /// <summary>
        /// Creates the window header.
        /// </summary>
        /// <returns>The window header.</returns>
        private VisualElement CreateHeader()
        {
            VisualElement header = new VisualElement();
            header.AddToClassList("header");

            ObjectField animatorField = new ObjectField("DT Animator")
            {
                objectType = typeof(DTAnimator),
                allowSceneObjects = true
            };
            if(controller != null && controller.animator != null)
            {
                animatorField.SetValueWithoutNotify(controller.animator);
            }

            nameField = new TextField();
            nameField.name = "sequencer-name";
            nameField.SetPlaceHolderText(SequencerVals.sequencerNamePlaceholder);

            VisualElement buttons = new VisualElement();
            buttons.name = "header-buttons";
            Button clearButton = new Button(controller.ClearSequencer)
            {
                text = "Clear"
            };
            Button saveButton = new Button(controller.SaveSequencer)
            {
                text = "Save"
            };
            Button loadButton = new Button(controller.LoadSequencer)
            {
                text = "Load"
            };
            Button deleteButton = new Button(controller.DeleteSequencer)
            {
                text = "Delete"
            };
            buttons.Add(clearButton);
            buttons.Add(saveButton);
            buttons.Add(loadButton);
            buttons.Add(deleteButton);

            header.Add(animatorField);
            header.Add(nameField);
            header.Add(buttons);

            return header;
        }
        
        /// <summary>
        /// Creates the window's main body.
        /// </summary>
        /// <returns>The window body.</returns>
        private VisualElement CreateSequencerBody()
        {
            VisualElement sequencerBody = new VisualElement();
            sequencerBody.name = "sequencer-body";
            sequenceContainer = new ScrollView(ScrollViewMode.Vertical);
            sequenceContainer.name = "sequence-container";
            tweenContainer = new ScrollView(ScrollViewMode.VerticalAndHorizontal);
            tweenContainer.name = "tween-container";

            sequenceContainer.verticalScroller.valueChanged += _ =>
            {
                tweenContainer.verticalScroller.value = sequenceContainer.verticalScroller.value;
            };
            tweenContainer.verticalScroller.valueChanged += _ =>
            {
                sequenceContainer.verticalScroller.value = tweenContainer.verticalScroller.value;
            };

            sequencerBody.Add(sequenceContainer);
            sequencerBody.Add(tweenContainer);
            sequenceContainer.Add(
                new Button(controller.CreateSequence)
                {
                    text = "Create Sequence"
                }
            );

            return sequencerBody;
        }

        /// <summary>
        /// Creates a new sequence
        /// </summary>
        /// <param name="sequenceController">The controller to create the view on</param>
        internal void CreateSequence(SequenceController sequenceController)
        {
            sequenceController.visualElement = CreateSequenceElement();
            int insertIndex = sequenceContainer.childCount - 1;

            sequenceController.visualElement.Insert(0, CreateTwequenceHeader());
            sequenceContainer.Insert(insertIndex, sequenceController.visualElement);

            if (insertIndex == 0)
            {
                ToggleJoinToggleVisibility(sequenceController.visualElement);
            }

            sequenceController.tweenList = new VisualElement();
            sequenceController.tweenList.AddToClassList(SequencerVals.tweenList);
            sequenceController.tweenList.Add(new Button(() => controller.CreateTween(sequenceController))
            {
                text = "Create Tween"
            });

            tweenContainer.Add(sequenceController.tweenList);

            TextField sequenceName = sequenceController.visualElement.SearchChildren<TextField>(SequencerVals.sequenceName);
            FloatField delayField = sequenceController.visualElement.SearchChildren<FloatField>(SequencerVals.delayName);
            Button deleteButton = sequenceController.visualElement.SearchChildren<Button>(SequencerVals.deleteButtonName);
            Toggle joinToggle = sequenceController.visualElement.SearchChildren<Toggle>(SequencerVals.joinToggleName);
            IntegerField loopField = sequenceController.visualElement.SearchChildren<IntegerField>(SequencerVals.loopField);

            sequenceName.RegisterValueChangedCallback(sequenceController.OnNameChanged);
            delayField.RegisterValueChangedCallback(sequenceController.OnDelayChanged);
            deleteButton.clicked += () => controller.DeleteSequence(sequenceController);
            joinToggle.RegisterValueChangedCallback(evt => controller.JoinSequence(evt.newValue, sequenceController));
            loopField.RegisterValueChangedCallback(sequenceController.OnLoopsChanged);
        }

        /// <summary>
        /// Creates the visual component of the sequence.
        /// </summary>
        /// <param name="sequenceName">TextField to place the sequencer name.</param>
        /// <param name="delayField">FloatField to place the sequencer delay.</param>
        /// <returns>The sequence's visual element.</returns>
        private VisualElement CreateSequenceElement()
        {
            VisualElement sequenceElement = new VisualElement();
            sequenceElement.AddToClassList(SequencerVals.sequenceClass);

            TextField sequenceName = new TextField();
            sequenceName.name = SequencerVals.sequenceName;
            sequenceName.SetPlaceHolderText(SequencerVals.sequenceNamePlaceholder);

            FloatField delayField = new FloatField("Delay");
            delayField.name = SequencerVals.delayName;

            IntegerField loopField = new IntegerField("Loops");
            loopField.name = SequencerVals.loopField;

            Toggle customGeneration = new Toggle("Contains Custom Generation");
            customGeneration.name = SequencerVals.customGeneration;

            Button demoButton = new Button(() => controller.DemoSequence(sequenceName.value))
            {
                text = "Demo",
                name = SequencerVals.demoButtonName
            };

            sequenceElement.Add(sequenceName);
            sequenceElement.Add(delayField);
            sequenceElement.Add(loopField);
            sequenceElement.Add(customGeneration);
            //sequenceElement.Add(demoButton);
            return sequenceElement;
        }

        /// <summary>
        /// Create a new Tween attached to this sequence controller.
        /// </summary>
        /// <param name="sequenceController">The sequence this tween belongs to.</param>
        internal void CreateTween(SequenceController sequenceController)
        {
            VisualElement tweenElement = CreateTweenElement();

            // Hide the join toggle if it's first in line
            if(sequenceController.tweenControllers.Count == 0)
            {
                ToggleJoinToggleVisibility(tweenElement);
            }

            // Store and display the data
            sequenceController.tweenList.Insert(sequenceController.tweenList.childCount - 1, tweenElement);
            TweenController tweenController = new TweenController()
            {
                visualElement = tweenElement,
                tween = new DTTween(),
                sequenceController = sequenceController
            };
            sequenceController.tweenControllers.Add(tweenController);

            FindTweenFields(tweenElement, out EnumField tweenTypeField, out FloatField durationField, out FloatField delayField,
                out EnumField easeField, out Button deleteButton, out Toggle joinToggle, out IntegerField loopField);

            // Establish Callbacks
            tweenTypeField.RegisterValueChangedCallback(tweenController.OnTweenTypeChanged);
            durationField.RegisterValueChangedCallback(tweenController.OnDurationChanged);
            delayField.RegisterValueChangedCallback(tweenController.OnDelayChanged);
            easeField.RegisterValueChangedCallback(tweenController.OnEaseChanged);
            deleteButton.clicked += () => controller.DeleteTween(sequenceController, tweenController);
            joinToggle.RegisterValueChangedCallback(evt => controller.JoinTween(evt.newValue, tweenController));
            loopField.RegisterValueChangedCallback(tweenController.OnLoopsChanged);
        }

        private void FindTweenFields(VisualElement tweenElement, out EnumField tweenTypeField, out FloatField durationField,
            out FloatField delayField, out EnumField easeField, out Button deleteButton, out Toggle joinToggle, out IntegerField loopField)
        {
            tweenTypeField = tweenElement.SearchChildren<EnumField>(SequencerVals.tweenTypeName);
            durationField = tweenElement.SearchChildren<FloatField>(SequencerVals.durationName);
            delayField = tweenElement.SearchChildren<FloatField>(SequencerVals.delayName);
            easeField = tweenElement.SearchChildren<EnumField>(SequencerVals.easeTypeName);
            deleteButton = tweenElement.SearchChildren<Button>(SequencerVals.deleteButtonName);
            joinToggle = tweenElement.SearchChildren<Toggle>(SequencerVals.joinToggleName);
            loopField = tweenElement.SearchChildren<IntegerField>(SequencerVals.loopField);
        }

        /// <summary>
        /// Create the visual component of the tween.
        /// </summary>
        /// <param name="tweenTypeField">EnumField for the tween type.</param>
        /// <param name="durationField">FloatField for the duration.</param>
        /// <param name="delayField">FloatField for the delay.</param>
        /// <param name="easeField">EnumField for the ease type.</param>
        /// <param name="joinToggle">Toggle for joining.</param>
        /// <param name="deleteButton">Button for deleting.</param>
        /// <returns>The tween's visual element.</returns>
        private VisualElement CreateTweenElement()
        {
            VisualElement tweenElement = new VisualElement();
            tweenElement.AddToClassList("tween");          

            // Set up the fields
            EnumField tweenTypeField = new EnumField("Tween Type", TweenType.Unset);
            tweenTypeField.name = SequencerVals.tweenTypeName;
            FloatField durationField = new FloatField("Duration");
            durationField.name = SequencerVals.durationName;
            FloatField delayField = new FloatField("Delay");
            delayField.name = SequencerVals.delayName;
            IntegerField loopField = new IntegerField("Loops");
            loopField.name = SequencerVals.loopField;
            EnumField easeField = new EnumField("Ease", Ease.Unset);
            easeField.name = SequencerVals.easeTypeName;
            VisualElement startEnd = new VisualElement();
            startEnd.AddToClassList("start-end-field");

            // Add all the fields
            tweenElement.Add(CreateTwequenceHeader());
            tweenElement.Add(tweenTypeField);
            tweenElement.Add(durationField);
            tweenElement.Add(delayField);
            tweenElement.Add(loopField);
            tweenElement.Add(easeField);
            tweenElement.Add(startEnd);

            return tweenElement;
        }

        /// <summary>
        /// Create Header for tweens and sequences.
        /// </summary>
        /// <param name="joinToggle">Toggle for joining.</param>
        /// <param name="deleteButton">Button for deleting.</param>
        /// <returns></returns>
        private VisualElement CreateTwequenceHeader()
        {
            VisualElement header = new VisualElement();
            header.AddToClassList("tween-header");

            Toggle joinToggle = new Toggle("Join");
            joinToggle.name = SequencerVals.joinToggleName;

            Button deleteButton = new Button()
            {
                text = "X"
            };
            deleteButton.name = SequencerVals.deleteButtonName;

            header.Add(joinToggle);
            header.Add(deleteButton);
            return header;
        }

        /// <summary>
        /// Hides or unhides the join toggle for tweens and sequences.
        /// </summary>
        /// <param name="twequence">The visual element for the tween or sequence.</param>
        internal void ToggleJoinToggleVisibility(VisualElement twequence)
        {
            VisualElement toggle = twequence[0][0];
            if (toggle.ClassListContains("hidden"))
            {
                toggle.RemoveFromClassList("hidden");
            }
            else
            {
                toggle.AddToClassList("hidden");
            }
        }

        /// <summary>
        /// Clears the sequencer body.
        /// </summary>
        internal void Clear()
        {
            nameField.ClearToPlaceHolder(SequencerVals.sequencerNamePlaceholder);
            rootVisualElement.Remove(sequencerBody);
            sequencerBody = CreateSequencerBody();
            rootVisualElement.Add(sequencerBody);
        }
    
        /// <summary>
        /// Generate the view from existing data.
        /// </summary>
        /// <param name="sequenceControllers"></param>
        internal void GenerateView(List<SequenceController> sequenceControllers)
        {
            Clear();
            foreach(SequenceController sequenceController in sequenceControllers)
            {
                // Create the sequence
                CreateSequence(sequenceController);
                sequenceController.isJoined = sequenceController.sequence.isJoined;
                if (sequenceController.isJoined)
                {
                    controller.JoinSequence(true, sequenceController);
                }

                Toggle seqJoinToggle = sequenceController.visualElement.SearchChildren<Toggle>(SequencerVals.joinToggleName);
                TextField seqNameField = sequenceController.visualElement.SearchChildren<TextField>(SequencerVals.sequenceName);
                FloatField seqDelayField = sequenceController.visualElement.SearchChildren<FloatField>(SequencerVals.delayName);
                IntegerField seqLoopField = sequenceController.visualElement.SearchChildren<IntegerField>(SequencerVals.loopField);

                seqJoinToggle.SetValueWithoutNotify(sequenceController.isJoined);
                seqNameField.SetValueWithoutNotify(sequenceController.sequence.name);
                seqDelayField.SetValueWithoutNotify(sequenceController.sequence.delay);
                seqLoopField.SetValueWithoutNotify(sequenceController.sequence.loops);

                // Create the tweens
                bool firstTween = true;
                foreach(TweenController tweenController in sequenceController.tweenControllers)
                {
                    // Set up the element
                    tweenController.visualElement = CreateTweenElement();
                    sequenceController.tweenList.Insert(sequenceController.tweenList.childCount - 1, tweenController.visualElement);
                    tweenController.isJoined = tweenController.tween.isJoined;
                    if (firstTween)
                    {
                        ToggleJoinToggleVisibility(tweenController.visualElement);
                        firstTween = false;
                    }
                    else if (tweenController.isJoined)
                    {
                        controller.JoinTween(true, tweenController);
                    }
                    tweenController.SetStartEndField(tweenController.tween.tweenType);
                    if(tweenController.tween.ease == Ease.INTERNAL_Custom)
                    {
                        tweenController.AddCurveField();
                    }

                    // Populate the fields
                    FindTweenFields(tweenController.visualElement, out EnumField tweenTypeField, out FloatField durationField,
                        out FloatField delayField, out EnumField easeField, out Button deleteButton, out Toggle joinToggle, 
                        out IntegerField loopField);

                    tweenTypeField.SetValueWithoutNotify(tweenController.tween.tweenType);
                    durationField.SetValueWithoutNotify(tweenController.tween.duration);
                    delayField.SetValueWithoutNotify(tweenController.tween.delay);
                    easeField.SetValueWithoutNotify(tweenController.tween.ease);
                    joinToggle.SetValueWithoutNotify(tweenController.isJoined);
                    loopField.SetValueWithoutNotify(tweenController.tween.loops);

                    // Establish Callbacks
                    tweenTypeField.RegisterValueChangedCallback(tweenController.OnTweenTypeChanged);
                    durationField.RegisterValueChangedCallback(tweenController.OnDurationChanged);
                    delayField.RegisterValueChangedCallback(tweenController.OnDelayChanged);
                    easeField.RegisterValueChangedCallback(tweenController.OnEaseChanged);
                    deleteButton.clicked += () => controller.DeleteTween(sequenceController, tweenController);
                    joinToggle.RegisterValueChangedCallback(evt => controller.JoinTween(evt.newValue, tweenController));
                    loopField.RegisterValueChangedCallback(tweenController.OnLoopsChanged);
                }
            }
        }
    }
}
