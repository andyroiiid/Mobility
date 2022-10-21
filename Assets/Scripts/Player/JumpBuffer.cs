using UnityEngine;

namespace Player
{
    public class JumpBuffer
    {
        private readonly float _time;

        private float _timer;

        public JumpBuffer(float time = 0.1f)
        {
            _time = time;
        }

        public void Update(float deltaTime)
        {
            _timer = Mathf.Max(0, _timer - deltaTime);
        }

        public void QueueJump()
        {
            _timer = _time;
        }

        public bool TryConsumeJump()
        {
            if (_timer <= 0) return false;

            _timer = 0;
            return true;
        }
    }
}