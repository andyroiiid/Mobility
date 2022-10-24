using UnityEngine;

namespace Player.Movement
{
    [RequireComponent(typeof(PlayerMovement))]
    public class PlayerJump : MonoBehaviour
    {
        [SerializeField] private float jumpSpeed = 9.0f;

        private PlayerMovement _movement;

        private JumpBuffer _jumpBuffer;
        private CoyoteTimer _coyoteTimer;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();

            _jumpBuffer = new JumpBuffer();
            _coyoteTimer = new CoyoteTimer();
        }

        public void Jump()
        {
            _jumpBuffer.QueueJump();
        }

        private void TryJump()
        {
            if (!_coyoteTimer.CanJump)
            {
                // leaving ground for too long
                return;
            }

            if (!_jumpBuffer.TryConsumeJump())
            {
                // no jump input buffered
                return;
            }

            _movement.Launch(Vector3.up * jumpSpeed, overrideY: true);
        }

        private void FixedUpdate()
        {
            _jumpBuffer.Update(Time.fixedDeltaTime);
            _coyoteTimer.Update(_movement.IsOnGround, Time.fixedDeltaTime);
            TryJump();
        }
    }
}