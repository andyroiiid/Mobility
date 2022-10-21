using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player.Input
{
    [CreateAssetMenu(menuName = "InputAction/Button")]
    public class InputActionButton : ScriptableObject
    {
        [SerializeField] private InputAction action;

        public void Enable(UnityAction pressed, UnityAction released)
        {
            action.Enable();

            action.performed += _ => pressed?.Invoke();
            action.canceled += _ => released?.Invoke();
        }
    }
}