using System;
using System.Collections.Generic;
using UnityEngine;

public class ChargingItem : MonoBehaviour, IStable
{
    public string ParentName { set => _parentName = value; }
    public bool IsUsingPool { set => _isUsingPool = value; }
    public event Action<ChargingItem> OnRelease;

    [SerializeField] private SpawnPrefabSO _prefabSO;
    [SerializeField] private int _colourIndex = 0;
    [SerializeField] private int _energyAmount = 10;
    [SerializeField] private bool _isLocalSceneObject = false;

    private bool _enableDestroy = false;
    private string _parentName;
    private bool _isUsingPool = false;

    private void OnEnable()
    {
        _enableDestroy = false;
    }

    public void TakeItem()
    {
        _enableDestroy = true;
        if (_isUsingPool)
        {
            OnRelease?.Invoke(this);
        }
        else
            Destroy(gameObject);
    }

    private void TakeItem(CharacteristicsController controller)
    {
        if (controller.TryCharge(_colourIndex, _energyAmount))
        {
            _enableDestroy = true;
            if (_isUsingPool)
            {
                OnRelease?.Invoke(this);
            }
            else
                Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacteristicsController controller))
        {
            TakeItem(controller);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent(out CharacteristicsController controller) && !_enableDestroy)
        {
            TakeItem(controller);
        }
    }

    private void OnDestroy()
    {
        if (_parentName!=null && SpawnedObjectsDict.spawnersSize.ContainsKey(_parentName))
            SpawnedObjectsDict.spawnersSize[_parentName]--;
        if (!_enableDestroy && !_isLocalSceneObject)
            SaveData();
    }
    
#region IStable
    public void SaveData()
    {
        var data = new Dictionary<string, object>
        {
            {"Position", transform.position},
            {"Rotation", transform.rotation},
            {"Parent", _parentName},
            {"Prefab", _prefabSO.SpawnPrefab},
        };
        var SavedObjectData = new LoadableParameters(data, LoadData);

        StableObjectsData.StableObjectsList.Add(SavedObjectData);
    }

    public void LoadData(LoadableParameters data)
    {
        var parameters = data.Parameters;

        if (SpawnedObjectsDict.spawnersSize.ContainsKey((string)parameters["Parent"]))
            SpawnedObjectsDict.spawnersSize[(string)parameters["Parent"]]++;

        var spawnedObject = Instantiate((GameObject)parameters["Prefab"]);
        spawnedObject.transform.SetPositionAndRotation((Vector3)parameters["Position"], (Quaternion)parameters["Rotation"]);

        if (spawnedObject.TryGetComponent(out ChargingItem item))
        {
            item.ParentName = (string)parameters["Parent"];
        }
        else
        {
            Debug.LogWarning("No controller found to load characteristics");
        }
    }
#endregion
}
