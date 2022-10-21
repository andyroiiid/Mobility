using UnityEngine;

namespace Extensions
{
    public static class TransformExtensions
    {
        public static void LerpPosition(this Transform transform, Vector3 targetPosition, float alpha)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, alpha);
        }

        public static void LerpRotation(this Transform transform, Quaternion targetRotation, float alpha)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, alpha);
        }
    }
}