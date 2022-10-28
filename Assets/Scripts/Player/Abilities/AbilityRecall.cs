using Extensions;
using Player.Movement;
using UnityEngine;
using Utilities;

namespace Player.Abilities
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerMovement))]
    public class AbilityRecall : MonoBehaviour
    {
        private Transform _transform;
        private PlayerController _controller;
        private PlayerMovement _movement;

        private bool _recalling;

        private readonly RingBuffer<TransformSnapshot> _records = new(500);
        private float _timeTillNextRecord;

        private void Awake()
        {
            _transform = transform;
            _controller = GetComponent<PlayerController>();
            _movement = GetComponent<PlayerMovement>();
        }

        public void BeginRecall()
        {
            _recalling = true;
            _controller.LockInput(true);
            _movement.enabled = false;
        }

        private void EndRecall()
        {
            _recalling = false;
            _controller.LockInput(false);
            _movement.enabled = true;
        }

        private void Update()
        {
            if (_recalling)
            {
                UpdateRecalling();
            }
            else
            {
                UpdateRecording();
            }
        }

        private void UpdateRecalling()
        {
            if (!_records.TryPopBack(out var lastRecord))
            {
                EndRecall();
            }
            else
            {
                _transform.position = lastRecord.Position;
                _transform.rotation = lastRecord.Rotation;
            }
        }

        private bool FirstRecordOutOfDate(float maxTimeDifference)
        {
            if (!_records.PeekFront(out var firstRecord))
            {
                return false;
            }

            return Time.time - firstRecord.Timestamp > maxTimeDifference;
        }

        private void UpdateRecording()
        {
            if (_timeTillNextRecord <= 0)
            {
                _records.TryPushBack(_transform.CreateSnapshot());
                if (FirstRecordOutOfDate(3.0f))
                {
                    _records.PopFront();
                }

                _timeTillNextRecord += 0.2f / Mathf.Max(1.0f, _movement.Velocity.magnitude);
            }

            _timeTillNextRecord -= Time.deltaTime;
        }
    }
}