using UnityEngine;

namespace Environment
{
    public enum InteractType
    {
        Use,
        PickUp
    }

    public abstract class Interactable : MonoBehaviour
    {
        public abstract InteractType GetInteractType();

        public abstract void OnGainFocus();

        public abstract void OnLoseFocus();
    }

    public abstract class Usable : Interactable
    {
        public abstract void OnUse();
    }

    public abstract class Pickable : Interactable
    {
        public abstract void OnPickUp();

        public abstract void OnDrop();

        public abstract void OnMove(Vector3 position, Quaternion rotation);
    }
}