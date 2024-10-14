using UnityEngine;
using System.Collections;

public abstract class Spawner<T> : MonoBehaviour where T : MonoBehaviour
{  
    [SerializeField] protected float _spawnInterval = 2f;

    protected bool _isSpawning = false;

    // public List<ObjectData> _spawnedObjectsData = new();
    protected abstract void Spawn();

    protected void StartSpawning()
    {
        if (!_isSpawning)
        {
            _isSpawning = true;
            StartCoroutine(SpawnObjectsWithDelay());
        }
    }
    protected void StopSpawning()
    {
        if (_isSpawning)
        {
            _isSpawning = false;
            StopCoroutine(SpawnObjectsWithDelay());
        }
    }

    private IEnumerator SpawnObjectsWithDelay()
    {
        while (_isSpawning)
        {
            yield return new WaitForSeconds(_spawnInterval);
            if (_isSpawning)
                Spawn();
        }
    }
}