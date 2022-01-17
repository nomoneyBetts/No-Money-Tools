using UnityEngine;
using UnityEditor;

namespace DT_Animation
{
    [CustomEditor(typeof(DTAnimator))]
    public class DTInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            if (target == null) return;
            DTAnimator animator = (DTAnimator)target;

            if (GUILayout.Button("Open Sequencer Window"))
            {
                DTSequencer.ShowWindow(animator);
            }
        }
    }
}
