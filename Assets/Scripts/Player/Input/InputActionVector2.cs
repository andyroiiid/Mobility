using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Input
{
    [CreateAssetMenu(menuName = "InputAction/Vector2")]
    public class InputActionVector2 : ScriptableObject
    {
        [SerializeField] private InputAction action;

        public void Enable(UnityAction<Vector2> changed)
        {
            action.Enable();

            void Callback(InputAction.CallbackContext ctx)
            {
                changed?.Invoke(ctx.ReadValue<Vector2>());
            }

            action.started += Callback;
            action.performed += Callback;
            action.canceled += Callback;
        }
    }
}