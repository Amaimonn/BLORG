using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    public event Action ChangeSceneButtonClicked;

    [Header("Options")]
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private InputActionReference _settingsToogleInput;

    [Header("Additional Live")]
    [SerializeField] private GameObject _additionalLivePanel;
    private bool _isGameOver = false;
    private int _livesCount = 1;

    [Header("Game over")]
    [SerializeField] private GameObject _gameOverPanel;
    [SerializeField] private PointsView _pointsView;
    [SerializeField] private TMP_Text _totalPoints;
    private string ViewPointsText {get => _pointsView.PointsText; }
    // private Action<InputAction.CallbackContext> _settingsToogle;
    // private readonly Stack<GameObject> _openedPanels = new();

    private void OnEnable()
    {
        _settingsPanel.SetActive(false);
        _additionalLivePanel.SetActive(false);
        _gameOverPanel.SetActive(false);
        _settingsToogleInput.action.performed += SettingsWindowToggle;
        ServiceLocator.Current.Get<EventBus>().Subscribe<GameOverCallback>(GameOverHandler);
    }

    private void OnDisable()
    {
        _settingsToogleInput.action.performed -= SettingsWindowToggle;
        ServiceLocator.Current.Get<EventBus>().Unsubscribe<GameOverCallback>(GameOverHandler);
    }

    public void OpenSettings()
    {
        if (!_isGameOver)
        {
            ServiceLocator.Current.Get<PlayerDataManager>().InputController.Wizard.Disable();
            _settingsPanel.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
            Pause();
        }
    }

    public void CloseSettings()
    {
        ServiceLocator.Current.Get<PlayerDataManager>().InputController.Wizard.Enable();
        _settingsPanel.SetActive(false);

        if (!_additionalLivePanel.activeSelf)
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        Resume();
    }

    public void ReturnToMainMenu()
    {
        Resume();
        ChangeSceneButtonClicked?.Invoke();
        ServiceLocator.Current.Get<EventBus>().Invoke(new GameExitCallback());
    }

    public void Respawn()
    {
        _isGameOver = false;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        _additionalLivePanel.SetActive(false);
        var player = ServiceLocator.Current.Get<PlayerDataManager>().PlayerObject;
        player.SetActive(true);
        Resume();
    }

    private void SettingsWindowToggle(InputAction.CallbackContext context = default)
    {
        if (_settingsPanel.activeSelf)
        {
            CloseSettings();
        }
        else
        {
            OpenSettings();
        }
    }

    private void GameOverHandler(GameOverCallback callback)
    {
        Pause();
        _isGameOver = true;
        _settingsPanel.SetActive(false);
        if (_livesCount > 0)
        {
            _livesCount--;
            _additionalLivePanel.SetActive(true);
        }
        else
        {
            _gameOverPanel.SetActive(true);
            _totalPoints.text = ViewPointsText;
        }
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Pause()
    {
        Time.timeScale = 0;
    }

    private void Resume()
    {
        Time.timeScale = 1;
    }
}
