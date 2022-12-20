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

        private void Start()
        {
            _targetPosition = _rigidbody.position;
            _targetRotation = _rigidbody.rotation;
        }

        private void FixedUpdate()
        {
            if (!_pickedUp) return;
            _rigidbody.velocity = (_targetPosition - _rigidbody.position) / Time.fixedDeltaTime;
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
            gameObject.layer = LayerMask.NameToLayer("Ignore Player");
            _rigidbody.useGravity = false;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        public override void OnDrop()
        {
            _pickedUp = false;
            gameObject.layer = LayerMask.NameToLayer("Default");
            _rigidbody.useGravity = true;
            _rigidbody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }

        public override void OnMove(Vector3 position, Quaternion rotation)
        {
            _targetPosition = position;
            _targetRotation = rotation;
        }
    }
}