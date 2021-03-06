using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class AudioMixerSetFloatNode : TweenNode
    {
        public AudioMixerSetFloatNode(AudioMixerSetFloatVertex vertex) : base(vertex)
        {
            title = "AudioSource Pitch Node";
            CreateNodePort("Name", Orientation.Horizontal, Direction.Input, typeof(string));
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
