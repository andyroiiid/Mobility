using System;
using Extensions;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerCrouch))]
    [RequireComponent(typeof(PlayerJump))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float groundAcceleration = 50.0f;
        [SerializeField] private float airAcceleration = 25.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float groundDrag = 10.0f;
        [SerializeField] private float crouchDrag = 25.0f;
        [SerializeField] private float airDrag = 5.0f;
        [SerializeField] private float fallDrag = 0.5f;

        [NonSerialized] public Vector2 MoveInput;

        public float EyeHeight => _physics.height - _physics.radius;
        public Vector3 Velocity => _velocity;
        public bool IsOnGround { get; private set; }

        private CharacterController _physics;
        private PlayerCrouch _crouch;
        private PlayerJump _jump;

        private Vector3 _velocity = Vector3.zero;
        private Vector3 _acceleration = Vector3.zero;

        private void Awake()
        {
            _physics = GetComponent<CharacterController>();
            _crouch = GetComponent<PlayerCrouch>();
            _jump = GetComponent<PlayerJump>();
        }

        public void Launch(Vector3 velocity, bool overrideX = false, bool overrideY = false, bool overrideZ = false)
        {
            _velocity.x = overrideX ? velocity.x : _velocity.x + velocity.x;
            _velocity.y = overrideY ? velocity.y : _velocity.y + velocity.y;
            _velocity.z = overrideZ ? velocity.z : _velocity.z + velocity.z;
        }

        public void Jump() => _jump.Jump();

        public void Crouch() => _crouch.Crouch();

        public void UnCrouch() => _crouch.UnCrouch();

        private void GroundCheck()
        {
            IsOnGround = _physics.FootCast(out _, 0.1f);
        }

        private void CalcHorizontalAcceleration(Vector2 direction, float acceleration, float drag)
        {
            _acceleration.x = direction.x * acceleration - _velocity.x * drag;
            _acceleration.z = direction.y * acceleration - _velocity.z * drag;
        }

        private void UpdateAcceleration()
        {
            _acceleration = Vector3.zero;

            var facing = -transform.eulerAngles.y * Mathf.Deg2Rad;
            var direction = MoveInput.Rotate2D(facing);

            if (IsOnGround)
            {
                var drag = _crouch.IsCrouching ? crouchDrag : groundDrag;
                CalcHorizontalAcceleration(direction, groundAcceleration, drag);
            }
            else
            {
                CalcHorizontalAcceleration(direction, airAcceleration, airDrag);
                _acceleration.y = -gravity - _velocity.y * fallDrag;
            }
        }

        private void FixedUpdate()
        {
            UpdateAcceleration();

            _velocity += Time.fixedDeltaTime * _acceleration;
            _physics.Move(_velocity * Time.fixedDeltaTime);
            _velocity = _physics.velocity;

            GroundCheck();
            if (IsOnGround)
            {
                // prevent the boost from stepping up
                _velocity.y = 0.0f;
            }
        }
    }
}