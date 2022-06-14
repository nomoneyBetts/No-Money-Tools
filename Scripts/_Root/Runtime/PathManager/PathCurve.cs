// Author: Austin Betts
// Compay: No Money Studios
// Date Signed: 6/14/2022
// https://www.nomoneystudios.com/

using System.Collections.Generic;
using UnityEngine;

namespace NoMoney
{
    public enum PathType { Linear, CatmullRom, CubicBezier }
    public enum PathSpace { Full3D, XY, XZ, YZ }
    public enum ControlType { Aligned, Mirrored, Free }

    public class PathCurve : ScriptableObject
    {
        public static float ArrowFadeRatio = 1f;
        public static float ArrowOffsetRatio = 1.25f;
        public static float ArrowSizeRatio = .5f;
        public static float CurveFadeRatio = .35f;
        public static float CurveThicknessRatio = 5f;
        public static float ControlSizeRatio = .5f;
        public static float MouseOverError = .5f;
        public static Vector3 Snap = Vector3.one * .5f;
        public static Color PointHoverColor = Color.magenta;
        public static Color PointSelectColor = Color.yellow;

        private struct CatmullRomSegment
        {
            public Vector3 a, b, c, d;
            public CatmullRomSegment(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
            {
                this.a = a;
                this.b = b;
                this.c = c;
                this.d = d;
            }
        }

        public Vector3[] CurvePoints;
        public List<Vector3> BezierControls = new List<Vector3>();
        public Transform Target;
        public string PathName = "NAME ME";

        // Path Settings
        public List<Vector3> Waypoints = new List<Vector3>();
        public PathType PType = PathType.Linear;
        public int Resolution = 10;
        public bool ClosePath;
        public bool StartFromCurrent;
        public bool GenerateOnAwake;
        public PathSpace Space;
        public float Alpha = 0f;

        // Editor Settings
        public Vector3 RelativeSpawn = Vector3.zero;
        public bool DrawIndices, MoveRelative, HideControls, RelativePlane;
        public float HandleSize = .75f;
        public ControlType CType = ControlType.Aligned;
        public Color PathColor = new Color(0.05322172f, 0.3232309f, 0.735849f);
        public Color PointColor = new Color(0.6267715f, 0.2856295f, 0.7830189f);
        public Color ControlColor = new Color(0.7264151f, 0.2375697f, 0.2375697f);

        /// <summary>
        /// Generates an array of points representing the curve and caches them for later use.
        /// </summary>
        /// <param name="targetPos">The target's current position. Used for starting from current position.</param>
        /// <returns>A cached array of points representing the curve.</returns>
        public Vector3[] WaypointsToCurve()
        {
            if (Target == null)
            {
                CurvePoints = new Vector3[0];
                return CurvePoints;
            }
            List<Vector3> temp = new List<Vector3>(Waypoints);
            if (StartFromCurrent) temp.Insert(0, Target.transform.position);
            if (ClosePath) temp.Add(temp[0]);

            int numSeg = temp.Count - 1;
            if (numSeg <= 0)
            {
                CurvePoints = new Vector3[0];
                return CurvePoints;
            }
            else
            {
                CurvePoints = new Vector3[1 + Resolution * numSeg];
            }

            switch (PType)
            {
                case PathType.Linear:
                    CurvePoints = temp.ToArray();
                    break;
                case PathType.CatmullRom:
                    if (ClosePath)
                    {
                        temp.Insert(0, temp[temp.Count - 2]);
                        temp.Add(temp[2]);
                    }
                    else
                    {
                        temp.Insert(0, temp[0]);
                        temp.Add(temp[temp.Count - 1]);
                    }
                    ComputeCRCurve();
                    break;
                case PathType.CubicBezier:
                    InjectBezierControls(temp);
                    ComputeCBCurve();
                    break;
            }
            return CurvePoints;

            void ComputeCRCurve()
            {
                int curvePointIndex = 0;
                float step = 1f / Resolution;
                CurvePoints[curvePointIndex++] = temp[1];
                for (int i = 1; i < temp.Count - 2; i++)
                {
                    CatmullRomSegment seg = GenerateCRSegment(i);
                    float curStep = step;
                    for (int j = 0; j < Resolution; j++)
                    {
                        CurvePoints[curvePointIndex++] =
                            curStep * curStep * curStep * seg.a +
                            curStep * curStep * seg.b +
                            curStep * seg.c +
                            seg.d;
                        curStep += step;
                    }
                }
            }

            CatmullRomSegment GenerateCRSegment(int i)
            {
                Vector3 p1, p2, p3, p0;

                p1 = temp[i];
                p0 = i - 1 < 0 ? p1 : temp[i - 1];
                p2 = temp[i + 1];
                p3 = i + 2 >= temp.Count ? p2 : temp[i + 2];

                float t01 = Mathf.Pow(Vector3.Distance(p0, p1), Alpha);
                float t12 = Mathf.Pow(Vector3.Distance(p1, p2), Alpha);
                float t23 = Mathf.Pow(Vector3.Distance(p2, p3), Alpha);

                Vector3 m1_special = t01 == 0 ? Vector3.zero : (p1 - p0) / t01;
                Vector3 m2_special = t23 == 0 ? Vector3.zero : (p3 - p2) / t23;

                Vector3 m1 = p2 - p1 + t12 * (m1_special - (p2 - p0) / (t01 + t12));
                Vector3 m2 = p2 - p1 + t12 * (m2_special - (p3 - p1) / (t12 + t23));

                CatmullRomSegment seg = new CatmullRomSegment(
                    2.0f * (p1 - p2) + m1 + m2,
                    -3.0f * (p1 - p2) - m1 - m1 - m2,
                    m1,
                    p1
                );

                return seg;
            }

            void ComputeCBCurve()
            {
                int curvePointIndex = 0;
                float step = 1f / Resolution;
                CurvePoints[curvePointIndex++] = temp[0];
                for(int i = 0; i < temp.Count - 1; i += 3)
                {
                    float curStep = step;
                    for(int j = 0; j < Resolution; j++)
                    {
                        float k = 1 - curStep;
                        float x =
                            k * k * k * temp[i].x +
                            3 * curStep * k * k * temp[i + 1].x +
                            3 * curStep * curStep * k * temp[i + 2].x +
                            curStep * curStep * curStep * temp[i + 3].x;
                        float y =
                            k * k * k * temp[i].y +
                            3 * curStep * k * k * temp[i + 1].y +
                            3 * curStep * curStep * k * temp[i + 2].y +
                            curStep * curStep * curStep * temp[i + 3].y;
                        float z =
                            k * k * k * temp[i].z +
                            3 * curStep * k * k * temp[i + 1].z +
                            3 * curStep * curStep * k * temp[i + 2].z +
                            curStep * curStep * curStep * temp[i + 3].z;

                        CurvePoints[curvePointIndex++] = new Vector3(x, y, z);
                        curStep += step;
                    }
                }
            }
        }

        public List<Vector3> GenerateBezierControls()
        {
            if (Target == null) return null;
            BezierControls = new List<Vector3>();
            List<Vector3> temp = new List<Vector3>(Waypoints);

            if (StartFromCurrent) temp.Insert(0, Target.transform.position);
            if (ClosePath) temp.Add(temp[0]);

            BezierControls = new List<Vector3>();
            for(int i = 0; i < temp.Count - 1; i++)
            {
                AddBezierControls(temp[i + 1], temp[i]);
            }

            return BezierControls;
        }

        public void AddWaypoint(Vector3 waypoint)
        {
            if (Target == null) return;
            Waypoints.Add(waypoint);

            List<Vector3> temp = new List<Vector3>(Waypoints);
            if (StartFromCurrent) temp.Insert(0, Target.transform.position);
            if (ClosePath) temp.Add(temp[0]);

            if (temp.Count > 1) AddBezierControls(temp[temp.Count - 1], temp[temp.Count - 2]);
        }

        public void RemoveWaypoint(int index)
        {
            Waypoints.RemoveAt(index);
            if (BezierControls.Count == 2) BezierControls.Clear();
            else if(BezierControls.Count > 0)
            {
                int c2 = 2 * index;
                int c1 = c2 - 1;
                if (c1 < 0) BezierControls.RemoveRange(c2, 2);
                else if (c2 >= BezierControls.Count) BezierControls.RemoveRange(c1 - 1, 2);
                else BezierControls.RemoveRange(c1, 2);
            }
        }

        public void ClearWaypoints()
        {
            int length = Waypoints.Count;
            for(int i = 0; i < length; i++)
            {
                RemoveWaypoint(0);
            }
        }

        public void ToggleStartFromCurrent(bool value)
        {
            StartFromCurrent = value;
            if (value)
            {
                PrependBezierControls(Waypoints[0], Target.position);
            }
            else
            {
                BezierControls.RemoveRange(0, 2);
            }
        }

        public void ToggleClosePath(bool value)
        {
            ClosePath = value;
            if (value)
            {
                Vector3 start = StartFromCurrent ? Target.position : Waypoints[0];
                AddBezierControls(start, Waypoints[^1]);
            }
            else
            {
                BezierControls.RemoveRange(Waypoints.Count - 2, 2);
            }
        }

        public Vector3[] DotweenWaypoints()
        {
            List<Vector3> temp = new List<Vector3>(Waypoints);
            if (PType == PathType.CubicBezier) InjectBezierControls(temp);
            return temp.ToArray();
        }

        private void AddBezierControls(Vector3 p1, Vector3 p2)
        {
            Vector3 normal = p1 - p2;
            float y = -(normal.x + normal.z) / normal.y;
            Vector3 perp = (new Vector3(normal.x, y, normal.z)).normalized;
            BezierControls.Add(p2 + 2f * perp);
            BezierControls.Add(p1 - 2f * perp);
        }

        private void PrependBezierControls(Vector3 p1, Vector3 p2)
        {
            Vector3 normal = p1 - p2;
            float y = -(normal.x + normal.z) / normal.y;
            Vector3 perp = (new Vector3(normal.x, y, normal.z)).normalized;
            BezierControls.Insert(0, p1 + perp);
            BezierControls.Insert(0, p2 - perp);
        }

        private void InjectBezierControls(List<Vector3> list)
        {
            int tempIndex = 1;
            for (int i = 0; i < BezierControls.Count; i += 2)
            {
                list.InsertRange(tempIndex, BezierControls.GetRange(i, 2));
                tempIndex += 3;
            }
        }
    }
}
