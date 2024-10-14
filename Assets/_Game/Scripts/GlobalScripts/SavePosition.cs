using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public static class SavePosition
{
    public static bool save;
    public static Vector3 posVector;
    public static Dictionary<string, Vector3> DictPositions = new Dictionary<string, Vector3>();
    public static void SetPos(GameObject obj)
    {
        var sceneName = SceneManager.GetActiveScene().name;
        if(DictPositions.TryGetValue(sceneName, out Vector3 pos)){
            obj.transform.position = pos;
            DictPositions.Remove(sceneName);
        }
    }
}

