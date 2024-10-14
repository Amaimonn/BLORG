using UnityEngine;

public class UIRootView : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    [SerializeField] private Transform _uiSceneHolder;
    
    private void Awake()
    {
        HideLoadingScreen();
    }

    public void ShowLoadingScreen()
    {
        _loadingScreen.SetActive(true);
    }

    public void HideLoadingScreen()
    {
        _loadingScreen.SetActive(false);
    }

    public void AttachSceneUI(GameObject sceneUI)
    {
        ClearSceneUI();
        sceneUI.transform.SetParent(_uiSceneHolder, false);
    }

    public void ClearSceneUI()
    {
        var childCount = _uiSceneHolder.childCount;
        for (var i = 0; i < childCount; i++)
        {
            Destroy(_uiSceneHolder.GetChild(i).gameObject);
        }
    }
}
