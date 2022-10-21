using System;
using Extensions;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float moveAcceleration = 100.0f;
        [SerializeField] private float jumpSpeed = 10.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float moveDrag = 10.0f;
        [SerializeField] private float fallDrag = 0.5f;

        [NonSerialized] public Vector2 MoveInput;

        private CharacterController _physics;

        private Vector3 _prevPosition;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _acceleration = Vector3.zero;

        private bool _isOnGround;

        private void Awake()
        {
            _physics = GetComponent<CharacterController>();
            _prevPosition = transform.position;
        }

        public void Jump()
        {
            if (!_isOnGround) return;
            _velocity.y = jumpSpeed;
        }

        private void GroundCheck()
        {
            var halfHeight = _physics.height / 2;
            var radius = _physics.radius;
            var origin = transform.position + Vector3.down * (halfHeight - radius);

            _isOnGround = Physics.SphereCast(origin, radius, Vector3.down, out _, 0.1f);
        }

        private void UpdateAcceleration()
        {
            var facing = -transform.eulerAngles.y * Mathf.Deg2Rad;
            var direction = MoveInput.Rotate2D(facing);

            _acceleration.x = direction.x * moveAcceleration;
            _acceleration.z = direction.y * moveAcceleration;
            _acceleration.y = -gravity;
        }

        private static float LerpToZero(float value, float alpha)
        {
            return Mathf.Lerp(value, 0, Time.fixedDeltaTime * alpha);
        }

        private void UpdateVelocity()
        {
            _velocity += Time.fixedDeltaTime * _acceleration;

            _velocity.x = LerpToZero(_velocity.x, moveDrag);
            _velocity.z = LerpToZero(_velocity.z, moveDrag);
            _velocity.y = LerpToZero(_velocity.y, fallDrag);
        }

        private void CalcVelocityBasedOnMovement()
        {
            var position = transform.position;
            _velocity = (position - _prevPosition) / Time.fixedDeltaTime;
            _prevPosition = position;
        }

        private void FixedUpdate()
        {
            GroundCheck();
            UpdateAcceleration();
            UpdateVelocity();

            _physics.Move(_velocity * Time.fixedDeltaTime);

            CalcVelocityBasedOnMovement();
        }
    }
}