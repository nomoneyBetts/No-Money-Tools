// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TrailResizeNode : TweenNode
    {
        public TrailResizeNode(TrailResizeVertex vertex) : base(vertex)
        {
            title = "Trail Resize Node";
            CreateNodePort("Start Width", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End Width", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
