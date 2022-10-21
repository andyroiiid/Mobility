using UnityEngine;

namespace Player
{
    public class CoyoteTimer
    {
        private readonly CharacterController _physics;
        private readonly float _coyoteTime;

        private float _timer;

        public bool CanJump => _timer > 0;

        public CoyoteTimer(CharacterController physics, float coyoteTime = 0.2f)
        {
            _physics = physics;
            _coyoteTime = coyoteTime;
        }

        public void Update(bool isOnGround, float deltaTime)
        {
            _timer = isOnGround ? _coyoteTime : Mathf.Max(0, _timer - deltaTime);
        }
    }
}