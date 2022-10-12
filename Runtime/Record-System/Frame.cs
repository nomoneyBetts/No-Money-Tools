using UnityEngine;

namespace NoMoney.RecordSystem
{
    public abstract class Frame
    {
        /// <summary>
        /// Time since the beginning of the recording.
        /// </summary>
        public readonly float Timestamp;
        /// <summary>
        /// How far through this frame we are.
        /// Negative values means rewinding, positive means playing.
        /// </summary>
        public float Interpolation;

        /// <summary>
        /// This is the only constructor to be called!
        /// Frames are created through Activator, and therefore will only
        /// receive a timestamp argument.
        /// </summary>
        /// <param name="timestamp">The time since the beginning of the recording.</param>
        public Frame(float timestamp)
        {
            Timestamp = timestamp;
        }

        /// <summary>
        /// Interpolate this frame to the next and apply to the target.
        /// </summary>
        /// <param name="target">The object to apply the interpolation to.</param>
        /// <param name="next">The next frame to interpolate towards.</param>
        public abstract void Interpolate(GameObject target, Frame next);

        /// <summary>
        /// Record any data needed from the target.
        /// </summary>
        public abstract void RecordData(GameObject target);
    }
}
