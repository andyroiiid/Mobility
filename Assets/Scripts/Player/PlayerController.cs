using Extensions;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerController : MonoBehaviour
    {
        private PlayerInput _input;
        private PlayerMovement _movement;

        private float _eyeHeight = 0.5f;

        private const float PitchLimit = 89.999f;
        private float _pitch;

        private void Awake()
        {
            _input = GetComponent<PlayerInput>();
            _movement = GetComponent<PlayerMovement>();

            _input.OnMove += OnMoveInput;
            _input.OnLook += OnLookInput;
            _input.OnJump += _movement.Jump;
        }

        private void OnMoveInput(Vector2 input)
        {
            _movement.MoveInput = input;
        }

        private void OnLookInput(Vector2 input)
        {
            transform.Rotate(Vector3.up, input.x);
            _pitch = Mathf.Clamp(_pitch - input.y, -PitchLimit, PitchLimit);
        }

        public Vector3 GetEyePosition()
        {
            return transform.position + Vector3.up * _eyeHeight;
        }

        public Quaternion GetEyeRotation()
        {
            var yaw = transform.eulerAngles.y;
            return Quaternion.Euler(_pitch, yaw, 0.0f);
        }
    }
}