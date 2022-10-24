using UnityEngine;

namespace Player.Movement
{
    public class CoyoteTimer
    {
        private readonly float _time;

        private float _timer;

        public bool CanJump => _timer > 0;

        public CoyoteTimer(float time = 0.2f)
        {
            _time = time;
        }

        public void Suppress()
        {
            _timer = 0.0f;
        }

        public void Update(bool isOnGround, float deltaTime)
        {
            _timer = isOnGround ? _time : Mathf.Max(0, _timer - deltaTime);
        }
    }
}