using Extensions;
using Player.Movement;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerController player;

        private PlayerMovement _playerMovement;

        private void Start()
        {
            _playerMovement = player.GetComponent<PlayerMovement>();

            transform.position = player.GetEyePosition();
            transform.rotation = player.GetEyeRotation();
        }

        private void Update()
        {
            var timeError = Time.time - Time.fixedTime;

            var targetPosition = player.GetEyePosition() + _playerMovement.Velocity * timeError;
            var targetRotation = player.GetEyeRotation();

            var lerp = Time.deltaTime * 30.0f;
            transform.LerpPosition(targetPosition, lerp);
            transform.LerpRotation(targetRotation, lerp);
        }
    }
}