using Extensions;
using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerCrouch : MonoBehaviour
    {
        [SerializeField] private float crouchHeight = 1.2f;

        private CharacterController _physics;
        private PlayerMovement _movement;

        private bool _isCrouching;

        public bool IsCrouching
        {
            get => _isCrouching;
            private set
            {
                _isCrouching = value;
                _physics.SetHeight(value ? crouchHeight : _standUpHeight);
            }
        }

        private bool _wantToCrouch;
        private float _standUpHeight;

        private void Awake()
        {
            _physics = GetComponent<CharacterController>();
            _movement = GetComponent<PlayerMovement>();
            _standUpHeight = _physics.height;
        }

        public void Crouch()
        {
            _wantToCrouch = true;
        }

        public void UnCrouch()
        {
            _wantToCrouch = false;
        }

        private void FixedUpdate()
        {
            if (_wantToCrouch && _movement.IsOnGround)
            {
                if (!IsCrouching)
                {
                    IsCrouching = true;
                }
            }
            else
            {
                if (IsCrouching && !_physics.HeadCast(out _, _standUpHeight - crouchHeight))
                {
                    IsCrouching = false;
                }
            }
        }
    }
}