using Extensions;
using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [SerializeField] private PlayerController player;

        private void Start()
        {
            transform.position = player.GetEyePosition();
            transform.rotation = player.GetEyeRotation();
        }

        private void Update()
        {
            var lerp = Time.deltaTime * 30.0f;
            transform.LerpPosition(player.GetEyePosition(), lerp);
            transform.LerpRotation(player.GetEyeRotation(), lerp);
        }
    }
}