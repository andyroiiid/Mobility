using Player.Movement;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _input;
        private PlayerMovement _movement;

        private const float PitchLimit = 89.999f;
        private float _pitch;

        private bool _inputLocked;
        private Vector2 _movementInputBeforeLocking;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();

            _input.OnMove += OnMoveInput;
            _input.OnLook += OnLookInput;
            _input.OnJump += OnJumpInput;
            _input.OnCrouch += OnCrouchInput;
            _input.OnUnCrouch += OnUnCrouchInput;
            _input.OnAbility += OnAbilityInput;
        }

        private void LockInput(bool locked)
        {
            // cache and clear movement input
            if (locked)
            {
                _movementInputBeforeLocking = _movement.MoveInput;
                _movement.MoveInput = Vector2.zero;
            }
            else
            {
                _movement.MoveInput = _movementInputBeforeLocking;
                _movementInputBeforeLocking = Vector2.zero;
            }

            _inputLocked = locked;
        }

        private void OnMoveInput(Vector2 input)
        {
            if (_inputLocked) return;
            _movement.MoveInput = input;
        }

        private void OnLookInput(Vector2 input)
        {
            if (_inputLocked) return;
            transform.Rotate(Vector3.up, input.x);
            _pitch = Mathf.Clamp(_pitch - input.y, -PitchLimit, PitchLimit);
        }

        private void OnJumpInput()
        {
            if (_inputLocked) return;
            _movement.Jump();
        }

        private void OnCrouchInput()
        {
            if (_inputLocked) return;
            _movement.Crouch();
        }

        private void OnUnCrouchInput()
        {
            if (_inputLocked) return;
            _movement.UnCrouch();
        }

        private void OnAbilityInput()
        {
            LockInput(!_inputLocked);
            _movement.enabled = !_inputLocked;
        }

        public Vector3 GetEyePosition()
        {
            return transform.position + Vector3.up * _movement.EyeHeight;
        }

        public Quaternion GetEyeRotation()
        {
            var yaw = transform.eulerAngles.y;
            return Quaternion.Euler(_pitch, yaw, 0.0f);
        }
    }
}