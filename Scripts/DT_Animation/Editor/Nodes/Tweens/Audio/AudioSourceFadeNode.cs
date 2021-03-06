using UnityEditor.Experimental.GraphView;

namespace NoMoney.DTAnimation
{
    public class AudioSourceFadeNode : TweenNode
    {
        public AudioSourceFadeNode(AudioSourceFadeVertex vertex) : base(vertex)
        {
            title = "AudioSource Fade Node";
            CreateNodePort("Start", Orientation.Horizontal, Direction.Input, typeof(float));
            CreateNodePort("End", Orientation.Horizontal, Direction.Input, typeof(float));
        }
    }
}
