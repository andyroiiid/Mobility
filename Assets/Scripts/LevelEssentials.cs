using UnityEngine;

public class LevelEssentials : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject playerCameraPrefab;

    private Transform _transform;

    private void Awake()
    {
        GameStatics.Register(ref GameStatics.LevelEssentials, this);

        _transform = transform;

        var position = _transform.position;
        var rotation = _transform.rotation;
        Instantiate(playerPrefab, position, rotation);
        Instantiate(playerCameraPrefab, position, rotation);
    }
}