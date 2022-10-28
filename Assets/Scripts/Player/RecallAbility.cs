using System.Collections.Generic;
using Player.Movement;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(PlayerController))]
    [RequireComponent(typeof(PlayerMovement))]
    public class RecallAbility : MonoBehaviour
    {
        private Transform _transform;
        private PlayerController _controller;
        private PlayerMovement _movement;

        private bool _recalling;

        private struct Record
        {
            public readonly Vector3 Position;
            public readonly Quaternion Rotation;

            public Record(Vector3 position, Quaternion rotation)
            {
                Position = position;
                Rotation = rotation;
            }
        }

        private readonly Stack<Record> _records = new();

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
                if (!_records.TryPop(out var lastRecord))
                {
                    EndRecall();
                    return;
                }

                _transform.position = lastRecord.Position;
                _transform.rotation = lastRecord.Rotation;

                _records.TryPop(out _);
                _records.TryPop(out _);
                _records.TryPop(out _);
            }
            else
            {
                _records.Push(new Record(_transform.position, _transform.rotation));
            }
        }
    }
}