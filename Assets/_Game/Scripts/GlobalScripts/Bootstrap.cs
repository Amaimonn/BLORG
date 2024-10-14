using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    public event Action SceneChangeRequest;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private Aiming _aiming;
    [SerializeField] private CameraBehaviour _cameraBehaviour;
    [SerializeField] private MutationListSO _mutationsSO;
    [SerializeField] private RenderPipelineAsset _renderPipelineAsset;
    [SerializeField] private GameController _uiScenePrefab;
    private static bool _isFirstEntry = true;

    public void Boot(UIRootView uiRoot)
    {
        var uiScene = Instantiate(_uiScenePrefab);
        uiRoot.AttachSceneUI(uiScene.gameObject);
        uiScene.ChangeSceneButtonClicked += () => SceneChangeRequest?.Invoke();
    }

    private void Awake()
    {
        if(_isFirstEntry)
        {
            ServiceLocator.Current.Register(new MutationManager(_mutationsSO));
            _isFirstEntry =  false;
        }
        
        _playerController.Initialize();
        if(_cameraBehaviour!= null)
            _cameraBehaviour.Initialize();
        if(_aiming!= null)
            _aiming.Initialize();

        if (SceneManager.GetActiveScene().name == Scenes.CAVE && _renderPipelineAsset!=null)
        {
            QualitySettings.renderPipeline = _renderPipelineAsset;
        }
        else
        {
            // QualitySettings.renderPipeline = GraphicsSettings.defaultRenderPipeline;
            QualitySettings.renderPipeline = null;
        }
    }
}
