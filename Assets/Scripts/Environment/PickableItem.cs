using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(MeshRenderer))]
    [RequireComponent(typeof(Rigidbody))]
    public class PickableItem : Pickable
    {
        private Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;

        private bool _pickedUp;

        private Vector3 _targetPosition;
        private Quaternion _targetRotation;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Update()
        {
            if (!_pickedUp) return;
            _rigidbody.position = _targetPosition;
            _rigidbody.rotation = _targetRotation;
        }

        public override InteractType GetInteractType()
        {
            return InteractType.PickUp;
        }

        public override void OnGainFocus()
        {
            _meshRenderer.material.color = Color.gray;
        }

        public override void OnLoseFocus()
        {
            _meshRenderer.material.color = Color.white;
        }

        public override void OnPickUp()
        {
            _pickedUp = true;
            _rigidbody.isKinematic = true;
        }

        public override void OnDrop()
        {
            _pickedUp = false;
            _rigidbody.isKinematic = false;
        }

        public override void OnMove(Vector3 position, Quaternion rotation)
        {
            _targetPosition = position;
            _targetRotation = rotation;
        }
    }
}