using System;
using UnityEngine;

public class MainMenuBoot : MonoBehaviour
{    
    public event Action SceneChangeRequest;
    [SerializeField] private UIController _uiScenePrefab;

    public void Boot(UIRootView uiRoot)
    {
        var uiScene = Instantiate(_uiScenePrefab);
        uiRoot.AttachSceneUI(uiScene.gameObject);
        uiScene.PlayButton.clicked += () => SceneChangeRequest?.Invoke();
    }
}
