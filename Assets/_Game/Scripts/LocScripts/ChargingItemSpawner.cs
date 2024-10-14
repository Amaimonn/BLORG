using System.Collections;
using UnityEngine;

public class ChargingItemSpawner : Spawner<ChargingItem>//, IStable
{
    [SerializeField] private SpawnPrefabSO _prefabSO;
    [SerializeField] private float minSpawnPosition;
    [SerializeField] private float maxSpawnPosition;
    [SerializeField] private float heighSpawnPosition;
    [SerializeField] private int amountOfItems = 10;

    private CustomObjectPool _spawnerPool;
    private bool isStarting = false;

    private void Start()
    {
        if (!SpawnedObjectsDict.spawnersSize.ContainsKey(name))
        {
            SpawnedObjectsDict.spawnersSize[name] = 0;
        }
        _spawnerPool = new CustomObjectPool(_prefabSO.SpawnPrefab, amountOfItems);
    }
    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    
    private void Update()
    {
        if (!isStarting && !_isSpawning && SpawnedObjectsDict.spawnersSize[name] < amountOfItems)
        {
            StartCoroutine(StartSpawningWithDelay());
        }
    }

    protected override void Spawn()
    {
        var spawnPosition = transform.position + new Vector3(Random.Range(minSpawnPosition, maxSpawnPosition),
            heighSpawnPosition, Random.Range(minSpawnPosition, maxSpawnPosition));
        var spawnRotation = transform.rotation * Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up);

        var item = _spawnerPool.Get();
        item.transform.SetPositionAndRotation(spawnPosition, spawnRotation);

        if (item.TryGetComponent(out ChargingItem chargingItem))
        {
            chargingItem.ParentName = name;
            chargingItem.IsUsingPool = true;
            // chargingItem.ParentPool = _spawnerPool;
            chargingItem.OnRelease += ReleaseItem;
            // if (SpawnedObjectsDict.spawnersSize.ContainsKey(name))
            //     SpawnedObjectsDict.spawnersSize[name]++;
            // _itemList.Add(chargingItem);
        }
        AddItemCount();
    }

    private IEnumerator StartSpawningWithDelay()
    {
        if (!isStarting)
        {
            isStarting = true;
            yield return new WaitForSeconds(Random.Range(0.0f, 5.0f));
            isStarting = false;
            StartSpawning();
        }
    }

    private void AddItemCount()
    {
        SpawnedObjectsDict.spawnersSize[name]++;
        if (SpawnedObjectsDict.spawnersSize[name] >= amountOfItems)
            StopSpawning();
    }

    private void ReleaseItem(ChargingItem item)
    {
        SpawnedObjectsDict.spawnersSize[name]--;
        _spawnerPool.Release(item.gameObject);
        if (SpawnedObjectsDict.spawnersSize[name] < amountOfItems)
            StartCoroutine(StartSpawningWithDelay());
        item.OnRelease -= ReleaseItem;
    }

// #region IStable
//     public void SaveData()
//     {
//         ObjectData SavedObjectData = new()
//         {
//             objType = typeof(ChargingItemSpawner),
//             someInt = _spawnedCount,
//             useAct = true,
//             act = LoadCount
//         };

//         StableObjectsData.StableObjectsList.Add(SavedObjectData);
//     }

//     public void LoadData(ObjectData data)
//     {
//     }

//     private void LoadCount(ObjectData data)
//     {
//         _spawnedCount = data.someInt;
//         Debug.Log($"{name} {data.someInt} loaded");
//     }
// #endregion
}