// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using UnityEngine;

namespace NoMoney
{
    [System.Serializable]
    internal class StringCurveDict : SerializableDictionary<string, PathCurve> { }
    
    public class PathManager : MonoBehaviour
    {
        #region Serialization
        [SerializeField]
        private StringCurveDict _curves = new StringCurveDict();
        [SerializeField]
        private PathCurve _selectedCurve;
        #endregion

        ~PathManager()
        {
            foreach (var pair in _curves) Destroy(pair.Value);
        }

        private void Awake()
        {
            if (_curves == null) GenerateCurves();
        }

        private void GenerateCurves()
        {
            foreach(PathCurve curve in _curves.Values)
            {
                if (string.IsNullOrEmpty(curve.PathName))
                {
                    Debug.LogWarning("One of your curves is missing a name");
                }
                if(curve.GenerateOnAwake) curve.WaypointsToCurve();
            }
        }

        /// <param name="name">The name of the path to retrieve.</param>
        /// <returns>The path with the given name.</returns>
        public PathCurve GetPath(string name) => _curves[name];

        /// <param name="name">The name of the path to retrieve.</param>
        /// <returns>The points representing the path.</returns>
        public Vector3[] GetPathPoints(string name)
        {
            PathCurve curve = _curves[name];
            if (curve.CurvePoints == null) curve.WaypointsToCurve();
            return curve.CurvePoints;
        }
    }
}
