using UnityEngine;

public class PlayerDataManager: IService
{
    public GameObject PlayerObject { get; private set; }
    public Transform PlayerTransform { get; private set; }
    public InputController InputController { get; private set; } = new();
    public bool IsPlaying = false;
    public bool IsPaused = false;
    public bool IsRunning = false;

    //public PlayerDataManager(PlayerController playerObject)
    //{
    //    PlayerObject = playerObject.gameObject;
    //    PlayerTransform = PlayerObject.transform;
    //}

    public void SetPlayerData(PlayerController playerController)
    {
        PlayerObject = playerController.gameObject;
        PlayerTransform = PlayerObject.transform;
    }
}