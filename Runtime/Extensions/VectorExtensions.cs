using UnityEngine;

public static class VectorExtensions
{
    /// <summary>
    /// Rotates a Vector2 according to the rotation matrix
    /// |cos(theta) -sin(theta)|
    /// |sin(theta)  cos(theta)|
    /// </summary>
    /// <param name="vec">(Extension) The vector to rotate.</param>
    /// <param name="degrees">The angle in degrees to rotate.</param>
    /// <returns>A new Vector2 rotated degrees.</returns>
    public static Vector2 RotateVector(this Vector2 vec, float degrees)
    {
        float rad = Mathf.Deg2Rad * degrees;
        Vector2 r1 = new(Mathf.Cos(rad), -Mathf.Sin(rad));
        Vector2 r2 = new(Mathf.Sin(rad), Mathf.Cos(rad));
        return new Vector2(Vector2.Dot(r1, vec), Vector2.Dot(r2, vec));
    }
}
