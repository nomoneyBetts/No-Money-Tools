using UnityEngine;

namespace Recording
{
    public class TransformFrame : Frame
    {
        public Vector3 Position, Scale;
        public Quaternion Rotation;

        public TransformFrame(float timestamp) : base(timestamp) { }

        public override void Interpolate(GameObject target, Frame next)
        {
            Transform trans = target.transform;
            if(next == null)
            {
                trans.SetPositionAndRotation(Position, Rotation);
                trans.localScale = Scale;
            }
            else
            {
                float interp = Mathf.Abs(Interpolation);
                TransformFrame tNext = (TransformFrame)next;
                trans.SetPositionAndRotation(
                    Vector3.Lerp(Position, tNext.Position, interp),
                    Quaternion.Lerp(Rotation, tNext.Rotation, interp)
                );
                trans.localScale = Vector3.Lerp(Scale, tNext.Scale, interp);
            }
        }

        public override void RecordData(GameObject target)
        {
            Transform trans = target.transform;
            Position = trans.position;
            Rotation = trans.rotation;
            Scale = trans.localScale;
        }
    }
}
