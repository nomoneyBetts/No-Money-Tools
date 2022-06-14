// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;
using DG.Tweening;
using DG.DOTweenEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;

namespace NoMoney.DTAnimation
{
    public abstract class TweenNode : DTAnimatorNode
    {
        private Tween _tween;

        public TweenNode(TweenVertex tweVertex) : base(tweVertex)
        {
            CreateNodePort("Input", Orientation.Horizontal, Direction.Input, typeof(Node), Port.Capacity.Multi);
            CreateNodePort("Output", Orientation.Horizontal, Direction.Output, typeof(Node));
            ExtensionButtons();

            if (this is CustomTweenNode) return;
            CreateNodePort("Events", Orientation.Horizontal, Direction.Input, typeof(ExposedEvent), Port.Capacity.Multi);
            CreateNodePort("Duration", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Delay", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Ease", Orientation.Horizontal, Direction.Input, typeof(Ease));
            CreateNodePort("Loops", Orientation.Horizontal, Direction.Input, typeof(int));

            if (this is PathNode) return;
            CreateNodePort("Target", Orientation.Horizontal, Direction.Input, typeof(Object));

            void ExtensionButtons()
            {
                Button play = new Button(() =>
                {
                    _tween = tweVertex.GenerateTween();
                    DOTweenEditorPreview.PrepareTweenForPreview(_tween);
                    DOTweenEditorPreview.Start();
                })
                {
                    text = "Play"
                };
                VisualElement ssWrapper = new VisualElement();
                ssWrapper.style.flexDirection = FlexDirection.Row;
                Button stop = new Button(() => DOTweenEditorPreview.Stop())
                {
                    text = "Stop"
                };
                stop.style.flexGrow = 1;
                Button start = new Button(() => DOTweenEditorPreview.Start())
                {
                    text = "Start"
                };
                start.style.flexGrow = 1;
                Button reset = new Button(() => { 
                    if (_tween.IsActive()) _tween.Kill();
                    tweVertex.SetDefaultValue();
                })
                {
                    text = "Reset"
                };
                reset.style.flexGrow = 1;
                ssWrapper.Add(stop);
                ssWrapper.Add(start);
                ssWrapper.Add(reset);

                extensionContainer.Add(play);
                extensionContainer.Add(ssWrapper);
                RefreshExpandedState();
            }
        }
    }
}
