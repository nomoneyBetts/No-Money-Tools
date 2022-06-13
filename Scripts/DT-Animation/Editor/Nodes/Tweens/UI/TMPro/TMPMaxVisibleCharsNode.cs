using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TMPMaxVisibleCharsNode : TweenNode
    {
        public TMPMaxVisibleCharsNode(TMPMaxVisibleCharsVertex vertex) : base(vertex)
        {
            title = "TMPro Max Visible Chars Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(int));
        }
    }
}
