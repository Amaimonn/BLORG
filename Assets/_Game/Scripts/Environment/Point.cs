using System.Collections;
using UnityEngine;

public class Point : MonoBehaviour
{
    [SerializeField] private int _count;
    [SerializeField] private float _rotationSpeed = 120.0f;
    [SerializeField] private float _liveTime = 15.0f;
    private bool isPicked = false;

    private void Start()
    {
        StartCoroutine(Spining());
    }

    private IEnumerator Spining()
    {
        while (_liveTime >= 0)
        {
            gameObject.transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
            yield return _liveTime -= Time.deltaTime;
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PointsController picker))
        {
            if(!isPicked)
                PickUp(picker);
        }
    }

    private void PickUp(PointsController picker)
    {
        picker.AddPoints(_count);
        isPicked  =  true;
        Destroy(gameObject);
    }
}
