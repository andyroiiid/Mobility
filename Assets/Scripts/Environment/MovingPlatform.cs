using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(MeshRenderer))]
    public class MovingPlatform : Usable
    {
        [SerializeField] private float movePeriod = 4.0f;
        [SerializeField] private Vector3 moveSpeed;
        [SerializeField] private float rotationSpeed;

        private Transform _transform;
        private Rigidbody _rigidbody;
        private MeshRenderer _meshRenderer;

        private bool _frozen;
        private float _timer;
        private bool _goingUp = true;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _timer = movePeriod;
        }

        private void Update()
        {
            if (_frozen) return;

            if (_timer <= 0.0f)
            {
                _goingUp = !_goingUp;
                _timer += movePeriod;
            }

            _timer -= Time.deltaTime;

            _rigidbody.velocity = _goingUp ? moveSpeed : -moveSpeed;
            _transform.position += _rigidbody.velocity * Time.deltaTime;

            _rigidbody.angularVelocity = Vector3.up * (rotationSpeed * Mathf.Deg2Rad);
            _transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        public override InteractType GetInteractType()
        {
            return InteractType.Use;
        }

        public override void OnUse()
        {
            _frozen = !_frozen;
        }

        public override void OnGainFocus()
        {
            _meshRenderer.material.color = Color.gray;
        }

        public override void OnLoseFocus()
        {
            _meshRenderer.material.color = Color.white;
        }
    }
}