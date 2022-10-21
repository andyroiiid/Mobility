using Player.Input;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private InputActionVector2 moveInput;
        [SerializeField] private InputActionVector2 lookInput;
        [SerializeField] private InputActionButton jumpInput;

        public UnityAction<Vector2> OnMove;
        public UnityAction<Vector2> OnLook;
        public UnityAction OnJump;

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            moveInput.Enable(OnMove);
            lookInput.Enable(OnLook);
            jumpInput.Enable(OnJump, null);
        }
    }
}