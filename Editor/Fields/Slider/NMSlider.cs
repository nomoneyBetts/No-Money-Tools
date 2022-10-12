// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;

namespace NoMoney.Editor
{
    public class NMSlider : NMBaseField<float>
    {
        #region Properties
        public float HighValue
        {
            get => _slider.highValue; 
            set
            {
                _slider.highValue = value;
                this.Q<Label>("high-value").text = value.ToString();
            }
        }
        public float LowValue
        {
            get => _slider.lowValue;
            set
            {
                _slider.lowValue = value;
                this.Q<Label>("low-value").text = value.ToString();
            }
        }
        public string HighLabel
        {
            get => this.Q<Label>("high-label").text;
            set => this.Q<Label>("high-label").text = value;
        }
        public string LowLabel
        {
            get => this.Q<Label>("low-label").text;
            set => this.Q<Label>("low-label").text = value;
        }
        private bool _showValueLabels;
        public bool ShowValueLabels
        {
            get => _showValueLabels;
            set
            {
                _showValueLabels = value;
                VisualElement labels = this.Q<VisualElement>("value-labels");
                labels.style.height = value ?
                    new StyleLength(StyleKeyword.Auto) :
                    new StyleLength(0f);
                labels.style.visibility = value ? Visibility.Visible : Visibility.Hidden;
            }

        }

        public override float value
        {
            get => _field.value;
            set
            {
                if (value > HighValue) value = HighValue;
                if (value < LowValue) value = LowValue;
                _slider.value = value;
                _field.value = value;
            }
        }
        #endregion

        private readonly Slider _slider;
        private readonly FloatField _field;

        public NMSlider()
        {
            VisualTreeAsset visualTree = Resources.Load<VisualTreeAsset>("NMSlider");
            visualTree.CloneTree(this);
            styleSheets.Add(Resources.Load<StyleSheet>("Slider"));

            _slider = this.Q<Slider>();
            _slider.RegisterValueChangedCallback(evt =>
            {
                _field.SetValueWithoutNotify(evt.newValue);
                CreateEvent(evt);
            });

            _field = this.Q<FloatField>();
            _field.RegisterValueChangedCallback(evt =>
            {
                float value = evt.newValue;
                if (value < LowValue)
                {
                    value = LowValue;
                    _field.SetValueWithoutNotify(value);
                }
                if (value > HighValue)
                {
                    value = HighValue;
                    _field.SetValueWithoutNotify(value);
                }
                _slider.SetValueWithoutNotify(value);
                CreateEvent(evt);
            });

            void CreateEvent(ChangeEvent<float> evt)
            {
                evt.StopPropagation();
                using ChangeEvent<float> @event = ChangeEvent<float>.GetPooled(evt.previousValue, evt.newValue);
                @event.target = this;
                SendEvent(@event);
            }
        }

        public NMSlider(string title) : this()
        {
            Title = title;
        }

        public override void SetValueWithoutNotify(float newValue)
        {
            if (newValue > HighValue) newValue = HighValue;
            if (newValue < LowValue) newValue = LowValue;
            _slider.SetValueWithoutNotify(newValue);
            _field.SetValueWithoutNotify(newValue);
        }

        #region UXML Factory
        public new class UxmlFactory : UxmlFactory<NMSlider, UxmlTraits> { }

        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            private readonly UxmlStringAttributeDescription _title =
                new UxmlStringAttributeDescription { name = "title", defaultValue = null };
            private readonly UxmlFloatAttributeDescription _highValue =
                new UxmlFloatAttributeDescription { name = "high-value", defaultValue = 10f };
            private readonly UxmlFloatAttributeDescription _lowValue =
                new UxmlFloatAttributeDescription { name = "low-value", defaultValue = 0f };
            private readonly UxmlStringAttributeDescription _highLabel =
                new UxmlStringAttributeDescription { name = "high-label", defaultValue = null };
            private readonly UxmlStringAttributeDescription _lowLabel =
                new UxmlStringAttributeDescription { name = "low-label", defaultValue = null };
            private readonly UxmlFloatAttributeDescription _value =
                new UxmlFloatAttributeDescription { name = "value", defaultValue = 0f };
            private readonly UxmlBoolAttributeDescription _showValueLabels = 
                new UxmlBoolAttributeDescription { name = "show-values", defaultValue = false };

            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }

            public override void Init(VisualElement element, IUxmlAttributes bag, CreationContext ctx)
            {
                base.Init(element, bag, ctx);
                NMSlider slider = (NMSlider)element;

                slider.Title = _title.GetValueFromBag(bag, ctx);
                slider.HighValue = _highValue.GetValueFromBag(bag, ctx);
                slider.LowValue = _lowValue.GetValueFromBag(bag, ctx);
                slider.HighLabel = _highLabel.GetValueFromBag(bag, ctx);
                slider.LowLabel = _lowLabel.GetValueFromBag(bag, ctx);
                slider.value = _value.GetValueFromBag(bag, ctx);
                slider.ShowValueLabels = _showValueLabels.GetValueFromBag(bag, ctx);
            }
        }
        #endregion

    }
}
