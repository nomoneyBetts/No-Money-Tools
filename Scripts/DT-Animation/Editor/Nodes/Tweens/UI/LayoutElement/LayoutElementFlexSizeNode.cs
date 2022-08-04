// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class LayoutElementFlexSizeNode : TweenNode
    {
        public LayoutElementFlexSizeNode(LayoutElementFlexSizeVertex vertex) : base(vertex)
        {
            title = "Layout Element Flexible Size Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
