public class SceneChangeCallback : PriorityCallback
{
    public readonly string SceneName;
    public SceneChangeCallback(string sceneName, int priority=0, object callback=null) : base(priority, callback)
    {
        SceneName = sceneName;
    }
}
