using UnityEngine;

namespace Environment
{
    public abstract class Usable : MonoBehaviour
    {
        public abstract void OnUse();

        public abstract void OnGainFocus();

        public abstract void OnLoseFocus();
    }
}