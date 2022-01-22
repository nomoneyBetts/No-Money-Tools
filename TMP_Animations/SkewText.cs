using UnityEngine;
using TMPro;

namespace TMPAnimations{
    /// <summary>
    /// Skews and Shears text along an animation curve.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class SkewText : MonoBehaviour
    {
        public AnimationCurve vertexCurve = new AnimationCurve(
            new Keyframe(0, 0), 
            new Keyframe(0.25f, 2.0f), 
            new Keyframe(0.5f, 0), 
            new Keyframe(0.75f, 2.0f),
            new Keyframe(1, 0f)
        );
        public float curveScale, shearAmount;

        private TMP_Text textComponent;

        private void Awake(){
            textComponent = GetComponent<TMP_Text>();
        }

        private void Update(){
            WarpText();
        }

        private AnimationCurve CopyAnimationCurve(AnimationCurve curve){
            AnimationCurve newCurve = new AnimationCurve();
            newCurve.keys = curve.keys;
            return newCurve;
        }

        private void WarpText(){
            vertexCurve.preWrapMode = WrapMode.Clamp;
            vertexCurve.postWrapMode = WrapMode.Clamp;

            Vector3[] verts;
            Matrix4x4 matrix;

            textComponent.havePropertiesChanged = true;
            float oldCurveScale = curveScale;
            float oldShearAmount = shearAmount;
            AnimationCurve oldCurve = CopyAnimationCurve(vertexCurve);

            textComponent.ForceMeshUpdate();

            TMP_TextInfo textInfo = textComponent.textInfo;
            float boundsMinX = textComponent.bounds.min.x;
            float boundsMaxX = textComponent.bounds.max.x;

            // loop through all the characters and warp them
            for(int i = 0; i < textInfo.characterCount; i++){
                // ignore invisible characters
                if(!textInfo.characterInfo[i].isVisible)
                    continue;

                // vertex of the character
                int vertIndex = textInfo.characterInfo[i].vertexIndex;
                // index of mesh used by character
                int matIndex = textInfo.characterInfo[i].materialReferenceIndex;

                // get draft vertices of mesh
                verts = textInfo.meshInfo[matIndex].vertices;

                Vector3 offsetMidBaseline = new Vector2(
                    (verts[vertIndex + 0].x + verts[vertIndex + 2].x) / 2, 
                    textInfo.characterInfo[i].baseLine
                );

                // apply offset
                verts[vertIndex] -= offsetMidBaseline;
                verts[vertIndex + 1] -= offsetMidBaseline;
                verts[vertIndex + 2] -= offsetMidBaseline;
                verts[vertIndex + 3] -= offsetMidBaseline;

                // Apply Shearing
                Vector3 topShear = new Vector3(
                    shearAmount * (textInfo.characterInfo[i].topRight.y - textInfo.characterInfo[i].baseLine),
                    0, 
                    0
                );
                Vector3 bottomShear = new Vector3(
                    shearAmount * (textInfo.characterInfo[i].baseLine - textInfo.characterInfo[i].bottomRight.y),
                    0,
                    0
                );

                verts[vertIndex] -= bottomShear;
                verts[vertIndex + 1] += topShear;
                verts[vertIndex + 2] += topShear;
                verts[vertIndex + 3] -= bottomShear;

                // Compute the angle of rotation for each character based on the animation curve
                float x0 = (offsetMidBaseline.x - boundsMinX) / (boundsMaxX - boundsMinX); // Character's position relative to the bounds of the mesh.
                float x1 = x0 + 0.0001f;
                float y0 = vertexCurve.Evaluate(x0) * curveScale;
                float y1 = vertexCurve.Evaluate(x1) * curveScale;

                Vector3 horizontal = new Vector3(1, 0, 0);
                Vector3 tangent = new Vector3(x1 * (boundsMaxX - boundsMinX) + boundsMinX, y1) - new Vector3(offsetMidBaseline.x, y0);

                float dot = Mathf.Acos(Vector3.Dot(horizontal, tangent.normalized)) * 57.2957795f;
                Vector3 cross = Vector3.Cross(horizontal, tangent);
                float angle = cross.z > 0 ? dot : 360 - dot;

                matrix = Matrix4x4.TRS(new Vector3(0, y0, 0), Quaternion.Euler(0, 0, angle), Vector3.one);

                verts[vertIndex + 0] = matrix.MultiplyPoint3x4(verts[vertIndex + 0]);
                verts[vertIndex + 1] = matrix.MultiplyPoint3x4(verts[vertIndex + 1]);
                verts[vertIndex + 2] = matrix.MultiplyPoint3x4(verts[vertIndex + 2]);
                verts[vertIndex + 3] = matrix.MultiplyPoint3x4(verts[vertIndex + 3]);

                verts[vertIndex + 0] += offsetMidBaseline;
                verts[vertIndex + 1] += offsetMidBaseline;
                verts[vertIndex + 2] += offsetMidBaseline;
                verts[vertIndex + 3] += offsetMidBaseline;
            }

            // assign the draft vertices to the final vertices
            for(int i = 0; i < textInfo.meshInfo.Length; i++){
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                textComponent.UpdateGeometry(meshInfo.mesh, i);
            } 
        }
    }
}
