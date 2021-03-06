using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class SpriteFadeNode : TweenNode
    {
        public SpriteFadeNode(SpriteFadeVertex vertex) : base(vertex)
        {
            title = "Sprite Fade Node";

            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
