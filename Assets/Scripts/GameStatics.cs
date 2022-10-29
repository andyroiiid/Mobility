using Player;
using UnityEngine;

public static class GameStatics
{
    public static LevelEssentials LevelEssentials => _levelEssentials;
    public static PlayerController PlayerController => _playerController;
    public static PlayerCamera PlayerCamera => _playerCamera;

    private static LevelEssentials _levelEssentials;
    private static PlayerController _playerController;
    private static PlayerCamera _playerCamera;

    public static void RegisterLevelEssentials(LevelEssentials levelEssentials)
    {
        Register(ref _levelEssentials, levelEssentials);
    }

    public static void RegisterPlayerController(PlayerController playerController)
    {
        Register(ref _playerController, playerController);
    }

    public static void RegisterPlayerCamera(PlayerCamera playerCamera)
    {
        Register(ref _playerCamera, playerCamera);
    }

    private static void Register<T>(ref T slot, T instance) where T : MonoBehaviour
    {
        if (slot == null)
        {
            slot = instance;
            Debug.Log($"{instance.name} registered to {typeof(T).Name}.");
        }
        else
        {
            Debug.LogError($"Multiple {typeof(T).Name} instances!");
            Object.Destroy(instance.gameObject);
        }
    }
}