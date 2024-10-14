using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private string _sceneName;
    
    private void Start()
    {
        SpawnStableObjects();
    }
    
    private void SpawnStableObjects()
    {
        foreach (LoadableParameters objData in StableObjectsData.StableObjectsList)
        {
            objData.OnLoadParameters?.Invoke(objData);
        }

        StableObjectsData.StableObjectsList.Clear();
    }
}