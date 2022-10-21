using System;
using Extensions;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float groundAcceleration = 50.0f;
        [SerializeField] private float airAcceleration = 25.0f;
        [SerializeField] private float jumpSpeed = 9.0f;
        [SerializeField] private float gravity = 20.0f;
        [SerializeField] private float groundDrag = 5.0f;
        [SerializeField] private float airDrag = 2.5f;
        [SerializeField] private float fallDrag = 0.5f;
        [SerializeField] private float crouchHeight = 1.2f;

        [NonSerialized] public Vector2 MoveInput;

        public float EyeHeight => _physics.height - _physics.radius;
        public Vector3 Velocity => _velocity;

        private CharacterController _physics;

#region Velocity

        private Vector3 _prevPosition;
        private Vector3 _velocity = Vector3.zero;
        private Vector3 _acceleration = Vector3.zero;

#endregion

#region Jump

        private bool _isOnGround;
        private bool _prevIsOnGround;
        private JumpBuffer _jumpBuffer;
        private CoyoteTimer _coyoteTimer;

#endregion

#region Crouch

        private bool _isCrouching;

        private bool IsCrouching
        {
            get => _isCrouching;
            set
            {
                _isCrouching = value;
                _physics.SetHeight(value ? crouchHeight : _standUpHeight);
            }
        }

        private bool _wantToStandUp;
        private float _standUpHeight;

#endregion

        private void Awake()
        {
            _physics = GetComponent<CharacterController>();
            _prevPosition = transform.position;
            _jumpBuffer = new JumpBuffer();
            _coyoteTimer = new CoyoteTimer();
            _standUpHeight = _physics.height;
        }

#region Jump

        private void GroundCheck()
        {
            _prevIsOnGround = _isOnGround;
            _isOnGround = _physics.FootCast(out _, 0.1f);
        }

        public void Jump()
        {
            _jumpBuffer.QueueJump();
        }

        private void TryJump()
        {
            if (IsCrouching)
            {
                // can't jump when crouching
                return;
            }

            if (!_coyoteTimer.CanJump)
            {
                // leaving ground for too long
                return;
            }

            if (_jumpBuffer.TryConsumeJump())
            {
                _velocity.y = jumpSpeed;
            }
        }

#endregion

#region Crouch

        public void Crouch()
        {
            IsCrouching = true;
        }

        public void UnCrouch()
        {
            _wantToStandUp = true;
        }

        private bool TryStandUp()
        {
            if (!IsCrouching)
            {
                // already standing
                return true;
            }

            if (_physics.HeadCast(out _, _standUpHeight - crouchHeight))
            {
                // not enough space to stand up
                return false;
            }

            IsCrouching = false;
            return true;
        }

#endregion

#region Velocity

        private void CalcHorizontalAcceleration(Vector2 direction, float acceleration, float drag)
        {
            _acceleration.x = direction.x * acceleration - _velocity.x * drag;
            _acceleration.z = direction.y * acceleration - _velocity.z * drag;
        }

        private void UpdateAcceleration()
        {
            var facing = -transform.eulerAngles.y * Mathf.Deg2Rad;
            var direction = MoveInput.Rotate2D(facing);

            if (_isOnGround)
            {
                CalcHorizontalAcceleration(direction, groundAcceleration, groundDrag);
            }
            else
            {
                CalcHorizontalAcceleration(direction, airAcceleration, airDrag);
            }

            _acceleration.y = -gravity - _velocity.y * fallDrag;
        }

        private void UpdateVelocity()
        {
            _velocity += Time.fixedDeltaTime * _acceleration;
        }

        private void CalcVelocityBasedOnMovement()
        {
            var position = transform.position;
            _velocity = (position - _prevPosition) / Time.fixedDeltaTime;

            if (_isOnGround && _prevIsOnGround)
            {
                // prevent the boost from stepping up
                _velocity.y = 0.0f;
            }

            _prevPosition = position;
        }

#endregion

        private void FixedUpdate()
        {
            _jumpBuffer.Update(Time.fixedDeltaTime);
            _coyoteTimer.Update(_isOnGround, Time.fixedDeltaTime);
            TryJump();

            if (_wantToStandUp && TryStandUp())
            {
                _wantToStandUp = false;
            }

            UpdateAcceleration();
            UpdateVelocity();

            _physics.Move(_velocity * Time.fixedDeltaTime);

            GroundCheck();
            CalcVelocityBasedOnMovement();
        }
    }
}