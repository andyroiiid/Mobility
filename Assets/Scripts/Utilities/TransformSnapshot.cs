using UnityEngine;

namespace Utilities
{
    public struct TransformSnapshot
    {
        public readonly Vector3 Position;
        public readonly Quaternion Rotation;
        public readonly float Timestamp;

        public TransformSnapshot(Vector3 position, Quaternion rotation, float timestamp)
        {
            Position = position;
            Rotation = rotation;
            Timestamp = timestamp;
        }

        public TransformSnapshot(Transform transform) : this(transform.position, transform.rotation, Time.time)
        {
        }
    }
}