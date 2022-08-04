// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class GraphicFadeNode : TweenNode
    {
        public GraphicFadeNode(GraphicFadeVertex vertex) : base(vertex)
        {
            title = "Graphic Fade Node";
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
