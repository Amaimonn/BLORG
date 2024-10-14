using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public Button PlayButton;
    public Button OptionsButton;
    public Button ExitButton;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        PlayButton = root.Q<Button>("PlayBtn");
        OptionsButton = root.Q<Button>("OptionsBtn");
        ExitButton = root.Q<Button>("ExitBtn");

        PlayButton.clicked += PlayButtonDown;
        ExitButton.clicked += ExitButtonDown;
    }

    private void PlayButtonDown()
    {   
        StableObjectsData.StableObjectsList.Clear();
        CurrentColourData.SetDefaultColour();
        SavePosition.DictPositions.Clear();
        // Destroy(MutationManager.Instance);
        // SceneManager.LoadScene(Scenes.GAME);
    }

    private void ExitButtonDown()
    {
        Application.Quit();
    }
}
