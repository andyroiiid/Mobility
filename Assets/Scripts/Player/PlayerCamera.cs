using Extensions;
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
            transform.position = player.GetEyePosition() + _playerMovement.Velocity * timeError;
            transform.LerpRotation(player.GetEyeRotation(), Time.deltaTime * 30.0f);
        }
    }
}