using System;
using Environment;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [SerializeField] private float interactDistance = 2.0f;

        private Collider _currentCollider;
        private Interactable _currentInteractable;
        private bool _isHoldingPickable;

        private void UpdateCurrentInteractable(Interactable newInteractable)
        {
            if (newInteractable == _currentInteractable) return; // still looking at the same usable object

            if (_currentInteractable)
            {
                _currentInteractable.OnLoseFocus();
            }

            if (newInteractable)
            {
                newInteractable.OnGainFocus();
            }

            _currentInteractable = newInteractable;
        }

        private void UpdateCurrentCollider(Collider newCollider)
        {
            if (newCollider == _currentCollider) return; // still looking at the same collider

            _currentCollider = newCollider;
            UpdateCurrentInteractable(newCollider != null ? newCollider.GetComponent<Interactable>() : null);
        }

        private Vector3 QueryHoldingPosition(Ray eyeRay, float distance)
        {
            if (Physics.BoxCast(
                    eyeRay.origin,
                    _currentCollider.bounds.extents,
                    eyeRay.direction,
                    out var hit,
                    Quaternion.LookRotation(eyeRay.direction),
                    distance,
                    LayerMask.NameToLayer("Player")
                ))
            {
                distance = hit.distance;
            }

            return eyeRay.GetPoint(distance);
        }

        private void Update()
        {
            var eyeRay = GameStatics.PlayerCamera.EyeRay;
            if (_isHoldingPickable)
            {
                var pickable = (Pickable)_currentInteractable;
                pickable.OnMove(
                    QueryHoldingPosition(eyeRay, 1.0f),
                    Quaternion.LookRotation(eyeRay.direction)
                );
            }
            else
            {
                UpdateCurrentCollider(Physics.Raycast(eyeRay, out var hit, interactDistance) ? hit.collider : null);
            }
        }

        private void InteractWithUsable()
        {
            var usable = (Usable)_currentInteractable;
            usable.OnUse();
        }

        private void InteractWithPickable()
        {
            var pickable = (Pickable)_currentInteractable;
            if (!_isHoldingPickable)
            {
                pickable.OnPickUp();
                _isHoldingPickable = true;
            }
            else
            {
                pickable.OnDrop();
                _isHoldingPickable = false;
            }
        }

        public void Interact()
        {
            if (!_currentInteractable) return;

            switch (_currentInteractable.GetInteractType())
            {
                case InteractType.Use:
                    InteractWithUsable();
                    break;
                case InteractType.PickUp:
                    InteractWithPickable();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}