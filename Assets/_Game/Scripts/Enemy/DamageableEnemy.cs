using UnityEngine;

[SelectionBase]
public class DamageableEnemy : ColourDamageable
{
	// public event System.Action<DamagableEnemy, int> OnDeath;
    public CustomObjectPool ParentPool { set => _parentPool = value; }
    public string ParentName { set => _parentName = value; }

    [Header("Points")]
    [SerializeField] private GameObjectFloatDictionary _pointsProbabilityDict;
    
    [Header("Spawn distance")]
    [SerializeField] private float _minSpawnPosition = 0.0f;
    [SerializeField] private float _maxSpawnPosition = 10.0f;

    private CustomObjectPool _parentPool;
    private string  _parentName;
    private bool IsDead => _health.IsEmpty();
    
    public override void TakeDamage(int damage)
    {
        _health.CurrentValue -= damage;
        if (IsDead)
            Death();
    }

    public override void TakeColourDamage(int damage, int colourIndex)
    {
        if (colourIndex == _colourIndex)
        {
            TakeDamage(damage);
        }
        else
        {
            var item = _parentPool.Get();
            if (item.TryGetComponent(out DamageableEnemy damageableEnemy))
            {
                damageableEnemy.ParentPool = _parentPool;
                damageableEnemy.ParentName = _parentName;
            }

            if (item.TryGetComponent(out EnemyAI enemyAI))
            {                
                var position = transform.position  + new Vector3(Random.Range(_minSpawnPosition,  _maxSpawnPosition), 
                    0.0f, Random.Range(_minSpawnPosition,  _maxSpawnPosition)); 
                enemyAI.SetPositionAndRotation(position, 
                    transform.rotation  * Quaternion.AngleAxis(Random.Range(0f,  360f), Vector3.up));
            }
            SpawnedObjectsDict.spawnersSize[_parentName]++;
        }
    }

    private void OnEnable()
    {
        _health.Refresh();
        // _health.OnIndicatorChanged += Death;
    }

    private void OnDisable()
    {
        SpawnedObjectsDict.spawnersSize[_parentName]--;
        // _health.OnIndicatorChanged -= Death;
        _parentPool?.Release(gameObject);
    }

    private void Death()
    {
        SpawnPoints();
        gameObject.SetActive(false);
    }

    private void SpawnPoints()
    {
        foreach (var pair in _pointsProbabilityDict)
        {
            var probability = Random.Range(0.0f, 1.0f);
            if (pair.Value >= probability)
                Instantiate(pair.Key, 
                            transform.position + new Vector3(Random.Range(_minSpawnPosition, _maxSpawnPosition),
                            4.0f,
                            Random.Range(_minSpawnPosition, _maxSpawnPosition)),
                            transform.rotation * Quaternion.AngleAxis(Random.Range(0f, 360f), Vector3.up));
        }
    }
}
