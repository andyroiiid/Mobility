using System.Collections;
using UnityEngine;

namespace Environment
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovingPlatform : MonoBehaviour
    {
        [SerializeField] private float movePeriod = 4.0f;
        [SerializeField] private Vector3 moveSpeed;
        [SerializeField] private float rotationSpeed;

        private Transform _transform;
        private Rigidbody _rigidbody;

        private bool _goingUp = true;

        private void Awake()
        {
            _transform = transform;
            _rigidbody = GetComponent<Rigidbody>();
        }

        private IEnumerator Start()
        {
            while (true)
            {
                yield return new WaitForSeconds(movePeriod);
                _goingUp = !_goingUp;
            }
            // ReSharper disable once IteratorNeverReturns
        }

        private void Update()
        {
            _rigidbody.velocity = _goingUp ? moveSpeed : -moveSpeed;
            _transform.position += _rigidbody.velocity * Time.deltaTime;

            _rigidbody.angularVelocity = Vector3.up * (rotationSpeed * Mathf.Deg2Rad);
            _transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }
}