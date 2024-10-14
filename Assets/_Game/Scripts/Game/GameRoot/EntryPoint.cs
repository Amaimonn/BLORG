using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryPoint
{
    private static EntryPoint _instance;
    private readonly Coroutines _coroutines;
    private readonly UIRootView  _uiRoot;
    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void AutoStartGame()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        ServiceLocator.Current.Register(new EventBus());
        ServiceLocator.Current.Register(new PlayerDataManager());
        
        _instance = new EntryPoint();
        _instance.RunGame();
    }

    private EntryPoint()
    {
        _coroutines = new GameObject("[COROUTINES]").AddComponent<Coroutines>();
        Object.DontDestroyOnLoad(_coroutines.gameObject);

        var UIRootPrefab  = Resources.Load<UIRootView>("UIRoot");
        _uiRoot = Object.Instantiate(UIRootPrefab);
        Object.DontDestroyOnLoad(_uiRoot.gameObject);
    }

    private void RunGame()
    {
#if UNITY_EDITOR
        var sceneName = SceneManager.GetActiveScene().name;
        if(sceneName == Scenes.GAME)
        {
            _coroutines.StartCoroutine(LoadAndStartGame());
            return;
        }

        if(sceneName == Scenes.MAIN_MENU)
        {
            _coroutines.StartCoroutine(LoadMainMenu());
            return;
        }

        if(sceneName != Scenes.BOOT)
        {
            return;
        }
#endif
        _coroutines.StartCoroutine(LoadAndStartGame());
    }

    private IEnumerator LoadAndStartGame()
    {
        _uiRoot.ShowLoadingScreen();
        
        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.GAME);

        yield return new WaitForSeconds(2);

        var bootstrap = Object.FindFirstObjectByType<Bootstrap>();
        if(bootstrap != null)
        {
            bootstrap.Boot(_uiRoot);
            bootstrap.SceneChangeRequest += () => _coroutines.StartCoroutine(LoadMainMenu());
        }
        else
        {
            _uiRoot.ClearSceneUI();
        }

        _uiRoot.HideLoadingScreen();
    }

    private IEnumerator LoadMainMenu()
    {
        _uiRoot.ShowLoadingScreen();

        yield return LoadScene(Scenes.BOOT);
        yield return LoadScene(Scenes.MAIN_MENU);

        // yield return new WaitForSeconds(0.1f);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        var menu = Object.FindFirstObjectByType<MainMenuBoot>();
        if(menu != null)
        {
            // _uiRoot.ClearSceneUI();
            menu.Boot(_uiRoot);
            menu.SceneChangeRequest += () => _coroutines.StartCoroutine(LoadAndStartGame());
        }
        
        _uiRoot.HideLoadingScreen();  
    }

    private IEnumerator LoadScene(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}
