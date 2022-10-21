using UnityEngine;

namespace Extensions
{
    public static class MathExtensions
    {
        public static Vector2 Rotate2D(this Vector2 vector, float radians)
        {
            var cos = Mathf.Cos(radians);
            var sin = Mathf.Sin(radians);
            return new Vector2(
                cos * vector.x - sin * vector.y,
                sin * vector.x + cos * vector.y
            );
        }
    }
}