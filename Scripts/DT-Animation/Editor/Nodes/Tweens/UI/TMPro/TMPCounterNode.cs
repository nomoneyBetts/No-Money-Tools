// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class TMPCounterNode : TweenNode
    {
        public TMPCounterNode(TMPCounterVertex vertex) : base(vertex)
        {
            title = "TMPro Counter Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(int));
            CreateNodePort("Culture Info", Orientation.Horizontal, Direction.Input, typeof(object));
            CreateNodePort("Thousands Separator", Orientation.Horizontal, Direction.Input, typeof(bool));
        }
    }
}
