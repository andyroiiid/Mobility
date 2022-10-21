using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField]
        private InputAction moveInput;

        [SerializeField]
        private InputAction lookInput;

        [SerializeField]
        private InputAction jumpInput;

        public UnityAction<Vector2> OnMove;
        public UnityAction<Vector2> OnLook;
        public UnityAction OnJump;

        private void Awake()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            moveInput.Enable();
            moveInput.started += OnMoveInput;
            moveInput.performed += OnMoveInput;
            moveInput.canceled += OnMoveInput;

            lookInput.Enable();
            lookInput.started += OnLookInput;
            lookInput.performed += OnLookInput;
            lookInput.canceled += OnLookInput;

            jumpInput.Enable();
            jumpInput.started += OnJumpInput;
        }

        private void OnMoveInput(InputAction.CallbackContext ctx)
        {
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
        }

        private void OnLookInput(InputAction.CallbackContext ctx)
        {
            OnLook?.Invoke(ctx.ReadValue<Vector2>());
        }

        private void OnJumpInput(InputAction.CallbackContext ctx)
        {
            OnJump?.Invoke();
        }
    }
}