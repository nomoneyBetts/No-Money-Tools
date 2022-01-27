using UnityEngine;
using TMPro;

namespace TMPAnimations{
    /// <summary>
    /// Animates text along a Sin Wave as to appear wavy.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class WavyText : MonoBehaviour
    {
#region Serialization
        [Header("X Parameters")]
        public float xAmplitude;
        public float xFrequency;
        public float xPeriod;

        [Header("Y Parameters")]
        public float yAmplitude;
        public float yFrequency;
        public float yPeriod;

        [Header("Z Parameters")]
        public float zAmplitude;
        public float zFrequency;
        public float zPeriod;
#endregion

        private TMP_Text text;

        private void Awake(){
            text = GetComponent<TMP_Text>();
        }

        private void Update(){
            AnimateTextBob();
        }

        private void AnimateTextBob(){
            // force update to get geometry
            text.ForceMeshUpdate();

            TMP_TextInfo textInfo = text.textInfo;

            // do nothing if there aren't chars
            if(textInfo.characterCount == 0){
                return;
            }

            // loop through all of the chars
            for(int i = 0; i < textInfo.characterCount; i++){
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                // do nothing for invisible chars
                if(!charInfo.isVisible){
                    continue;
                }

                // set the draft vertices
                Vector3[] verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
                for(int j = 0; j < 4; j++){
                    Vector3 orig = verts[charInfo.vertexIndex + j];
                    verts[charInfo.vertexIndex + j] += new Vector3(
                        Mathf.Sin(Time.time * xFrequency + orig.y * xPeriod) * xAmplitude, 
                        Mathf.Sin(Time.time * yFrequency + orig.x * yPeriod) * yAmplitude, 
                        Mathf.Sin(Time.time * zFrequency + orig.z * zPeriod) * zAmplitude
                    );
                }
            }   

            // assign the draft vertices to the final vertices
            for(int i = 0; i < textInfo.meshInfo.Length; i++){
                TMP_MeshInfo meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                text.UpdateGeometry(meshInfo.mesh, i);
            }    
        }
    }
}
