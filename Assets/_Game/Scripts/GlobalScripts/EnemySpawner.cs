using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner<EnemyAI>
{
    [SerializeField] private SpawnPrefabSO[] _enemyPrefabsSO;
    [SerializeField] private int _maxEnemy = 100;
    [SerializeField] private float _delayBeforeStart =  3.0f;
    [SerializeField] private float _spawnCooldown = 2.0f;
    [SerializeField] private float _spawnDistanceRatio = 0.9f;

    private readonly List<CustomObjectPool> _spawnerPools = new();

    private void Awake()
    {
        SpawnedObjectsDict.spawnersSize[name] = 0;

        foreach (var enemyPrefabSO in _enemyPrefabsSO)
        {
            _spawnerPools.Add(new CustomObjectPool(enemyPrefabSO.SpawnPrefab, _maxEnemy));
        }
    }

    private void OnEnable()
    {
        StartCoroutine(SpawnEnemies());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(_delayBeforeStart);
        
        while (true)
        {
            if (SpawnedObjectsDict.spawnersSize[name] < _maxEnemy)
            {
                Spawn();
            }

            yield return new WaitForSeconds(_spawnCooldown);
        }
    }

    protected override void Spawn()
    {
        var spawnPosition = Random.Range(0, 2) switch
        {
            0 => new Vector3(Random.Range(-153.6f, 153.6f) * _spawnDistanceRatio, 2.0f, 153.6f * (1 - 2 * Random.Range(0, 2)) * _spawnDistanceRatio),
            1 => new Vector3(153.6f * (1 - 2 * Random.Range(0, 2)) * _spawnDistanceRatio, 2.0f, Random.Range(-153.6f, 153.6f) * _spawnDistanceRatio),
            _ => new Vector3(0.0f, 10.0f, 0.0f),
        };

        var randomPoolIndex = Random.Range(0, _spawnerPools.Count);
        var item = _spawnerPools[randomPoolIndex].Get();
        item.transform.SetParent(transform);
        item.transform.position  =  spawnPosition;
        // item.transform.SetPositionAndRotation(position, 
        //     transform.rotation * Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up));
        if (item.TryGetComponent(out EnemyAI enemyAI))
        {
            enemyAI.SetPositionAndRotation(spawnPosition, transform.rotation * Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up));
        }

        if (item.TryGetComponent(out DamageableEnemy damageableEnemy))
        {
            damageableEnemy.ParentPool = _spawnerPools[randomPoolIndex];
            damageableEnemy.ParentName =  name;
        }

        SpawnedObjectsDict.spawnersSize[name]++;
    }
}
