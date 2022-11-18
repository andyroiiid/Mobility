using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private InputAction moveInput;
        [SerializeField] private InputAction lookInput;
        [SerializeField] private InputAction jumpInput;
        [SerializeField] private InputAction crouchInput;
        [SerializeField] private InputAction useInput;

        public UnityAction<Vector2> OnMove;
        public UnityAction<Vector2> OnLook;
        public UnityAction OnJump;
        public UnityAction OnCrouch;
        public UnityAction OnUnCrouch;
        public UnityAction OnUse;

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
            jumpInput.started += _ => OnJump?.Invoke();

            crouchInput.Enable();
            crouchInput.performed += _ => OnCrouch?.Invoke();
            crouchInput.canceled += _ => OnUnCrouch?.Invoke();

            useInput.Enable();
            useInput.performed += _ => OnUse?.Invoke();
        }

        private void OnMoveInput(InputAction.CallbackContext ctx)
        {
            OnMove?.Invoke(ctx.ReadValue<Vector2>());
        }

        private void OnLookInput(InputAction.CallbackContext ctx)
        {
            OnLook?.Invoke(ctx.ReadValue<Vector2>());
        }
    }
}