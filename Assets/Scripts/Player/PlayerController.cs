using Player.Movement;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))]
    [RequireComponent(typeof(PlayerInteract))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _input;
        private PlayerMovement _movement;
        private PlayerInteract _interact;

        private const float PitchLimit = 89.999f;
        private float _pitch;

        private bool _inputLocked;
        private Vector2 _cachedMovementInput;

        private void Awake()
        {
            GameStatics.RegisterPlayerController(this);

            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();
            _interact = GetComponent<PlayerInteract>();

            _input.OnMove += OnMoveInput;
            _input.OnLook += OnLookInput;
            _input.OnJump += OnJumpInput;
            _input.OnCrouch += OnCrouchInput;
            _input.OnUnCrouch += OnUnCrouchInput;
            _input.OnInteract += OnInteractInput;
        }

        public void LockInput(bool locked)
        {
            // cache and clear movement input
            if (locked)
            {
                _cachedMovementInput = _movement.MoveInput;
                _movement.MoveInput = Vector2.zero;
            }
            else
            {
                _movement.MoveInput = _cachedMovementInput;
                _cachedMovementInput = Vector2.zero;
            }

            _inputLocked = locked;
        }

        private void OnMoveInput(Vector2 input)
        {
            if (_inputLocked)
            {
                _cachedMovementInput = input;
            }
            else
            {
                _movement.MoveInput = input;
            }
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

        private void OnInteractInput()
        {
            if (_inputLocked) return;
            _interact.Interact();
        }

        public Vector3 GetPredictedEyePosition()
        {
            var timeError = Time.time - Time.fixedTime;
            return transform.position + Vector3.up * _movement.EyeHeight + _movement.Velocity * timeError;
        }

        public Quaternion GetEyeRotation()
        {
            var yaw = transform.eulerAngles.y;
            return Quaternion.Euler(_pitch, yaw, 0.0f);
        }
    }
}