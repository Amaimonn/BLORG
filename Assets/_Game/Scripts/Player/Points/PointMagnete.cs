using System.Collections;
using UnityEngine;

public class PointMagnete : MonoBehaviour
{
    [SerializeField] private Vector3 _magnetOffset = Vector3.zero;
    [SerializeField] private float _magnetePower = 100.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Point point))
        {
            StartCoroutine(MagnetePoint(point));
        }
    }

    private IEnumerator MagnetePoint(Point point)
    {
        while (point != null)
        {
            point.transform.position = Vector3.Lerp(point.transform.position, gameObject.transform.position + _magnetOffset,
                Time.fixedDeltaTime * _magnetePower);
            yield return new WaitForFixedUpdate();
        }
    }

}
