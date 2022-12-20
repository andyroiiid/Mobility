using UnityEngine;

namespace Extensions
{
    public static class PhysicsExtensions
    {
        public static void SetHeight(this CharacterController physics, float height)
        {
            physics.center = Vector3.up * height / 2;
            physics.height = height;
        }

        // dir should only be Vector3.up or Vector3.down
        private static bool SphereCast(this CharacterController physics, out RaycastHit hit, float dist, Vector3 dir)
        {
            var radius = physics.radius;
            var origin = physics.transform.position + physics.center + dir * (physics.height / 2 - radius);
            return Physics.SphereCast(origin, radius, dir, out hit, dist, LayerMask.GetMask("Default"));
        }

        public static bool HeadCast(this CharacterController physics, out RaycastHit hitInfo, float distance)
        {
            return SphereCast(physics, out hitInfo, distance, Vector3.up);
        }

        public static bool FootCast(this CharacterController physics, out RaycastHit hitInfo, float distance)
        {
            return SphereCast(physics, out hitInfo, distance, Vector3.down);
        }
    }
}