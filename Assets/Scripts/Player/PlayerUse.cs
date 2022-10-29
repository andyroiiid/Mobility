using Environment;
using UnityEngine;

namespace Player
{
    public class PlayerUse : MonoBehaviour
    {
        [SerializeField] private float useDistance = 4.0f;

        private Collider _currentCollider;
        private Usable _currentUsable;

        private void UpdateCurrentUsable(Usable newUsable)
        {
            if (newUsable == _currentUsable) return; // still looking at the same usable object

            if (_currentUsable)
            {
                _currentUsable.OnLoseFocus();
            }

            if (newUsable)
            {
                newUsable.OnGainFocus();
            }

            _currentUsable = newUsable;
        }

        private void UpdateCurrentCollider(Collider newCollider)
        {
            if (newCollider == _currentCollider) return; // still looking at the same collider

            _currentCollider = newCollider;
            UpdateCurrentUsable(newCollider != null ? newCollider.GetComponent<Usable>() : null);
        }

        private void Update()
        {
            var eyeRay = GameStatics.PlayerCamera.EyeRay;
            if (Physics.Raycast(eyeRay, out var hit, useDistance))
            {
                UpdateCurrentCollider(hit.collider);

                Debug.DrawLine(eyeRay.origin, hit.point, Color.green);
                Debug.DrawLine(hit.point + Vector3.down * 0.2f, hit.point + Vector3.up * 0.2f, Color.yellow);
                Debug.DrawLine(hit.point + Vector3.left * 0.2f, hit.point + Vector3.right * 0.2f, Color.yellow);
                Debug.DrawLine(hit.point + Vector3.back * 0.2f, hit.point + Vector3.forward * 0.2f, Color.yellow);
            }
            else
            {
                UpdateCurrentCollider(null);

                Debug.DrawRay(eyeRay.origin, eyeRay.direction * useDistance, Color.red);
            }
        }

        public void Use()
        {
            if (_currentUsable)
            {
                _currentUsable.OnUse();
            }
        }
    }
}