using Player;
using UnityEngine;

public static class GameStatics
{
    public static PlayerController PlayerController;
    public static PlayerCamera PlayerCamera;

    public static void Register<T>(ref T slot, T instance) where T : MonoBehaviour
    {
        if (slot == null)
        {
            slot = instance;
            Debug.Log($"{instance.name} registered to slot: {typeof(T).Name}.");
        }
        else
        {
            Debug.LogError($"Multiple {typeof(T).Name} instances!");
            Object.Destroy(instance.gameObject);
        }
    }
}