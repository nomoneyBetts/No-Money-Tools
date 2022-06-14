// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace NoMoney.DTAnimation
{
    public class RTransShakeAnchorPosNode : TweenNode
    {
        public RTransShakeAnchorPosNode(RTransShakeAnchorPosVertex vertex) : base(vertex)
        {
            title = "Rect Trans Shake Anchor Pos Node";
            CreateNodePort("Shake", Orientation.Horizontal, Direction.Input, typeof(Vector2));
            CreateNodePort("Vibrato", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Randomness", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("Snapping", Orientation.Horizontal, Direction.Input, typeof(bool));
            CreateNodePort("Fade Out", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
