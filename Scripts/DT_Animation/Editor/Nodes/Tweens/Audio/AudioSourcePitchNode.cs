using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class AudioSourcePitchNode : TweenNode
    {
        public AudioSourcePitchNode(AudioSourcePitchVertex vertex) : base(vertex)
        {
            title = "AudioSource Pitch Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
