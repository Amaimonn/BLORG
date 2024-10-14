using UnityEngine;
using UnityEngine.Pool;

public class CustomObjectPool
{
    private readonly ObjectPool<GameObject> _pool;

    private readonly GameObject _prefab;

    public CustomObjectPool(GameObject prefab, int prewarmObjectsCount)
    {
        _prefab = prefab;
        _pool = new ObjectPool<GameObject>(OnCreateObject, OnGetObject, OnRelease, OnObjectDestroy, false,
            prewarmObjectsCount);
    }

    public GameObject Get()
    {
        var obj = _pool.Get();
        return obj;
    }

    public void Release(GameObject obj)
    {
        _pool.Release(obj);
    }

    private void OnObjectDestroy(GameObject obj)
    {
        Object.Destroy(obj);
    }

    private void OnRelease(GameObject obj)
    {
        obj.SetActive(false);
        Debug.Log(obj.name.ToString() + " released");
    }

    private void OnGetObject(GameObject obj)
    {
        obj.SetActive(true);
    }

    private GameObject OnCreateObject()
    {
        return Object.Instantiate(_prefab);
    }
}