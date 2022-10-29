using Extensions;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        public Ray EyeRay => new(transform.position, transform.forward);

        private void Awake()
        {
            GameStatics.Register(ref GameStatics.PlayerCamera, this);
        }

        private void Start()
        {
            var player = GameStatics.PlayerController;

            transform.position = player.GetPredictedEyePosition();
            transform.rotation = player.GetEyeRotation();
        }

        private void Update()
        {
            var player = GameStatics.PlayerController;

            var targetPosition = player.GetPredictedEyePosition();
            var targetRotation = player.GetEyeRotation();

            var lerp = Time.deltaTime * 30.0f;
            transform.LerpPosition(targetPosition, lerp);
            transform.LerpRotation(targetRotation, lerp);
        }
    }
}