using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour, IMechanism
{
    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _rightDoor;
    [SerializeField] private AnimationCurve _openingCurve;
    [SerializeField] private AnimationCurve _closingCurve;
    [SerializeField] private Vector3 _openedLeftPosition;
    [SerializeField] private Vector3 _openingRightPosition;
    [SerializeField] private Vector3 _closePosition = Vector3.zero;

    private bool _doorsSwitcher = false;

    public void Activate()
    {
        StartCoroutine(MoveDoors(_openingCurve, _openedLeftPosition, _openingRightPosition));
    }

    public void Deactivate()
    {
        StartCoroutine(MoveDoors(_closingCurve, _closePosition, _closePosition));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController _))
        {
            Activate();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out PlayerController _))
        {
            Deactivate();
        }
    }
    
    private IEnumerator MoveDoors(AnimationCurve curve, Vector3 targetLeft, Vector3 targetRight)
    {
        _doorsSwitcher = !_doorsSwitcher; // (is opening <-> is closing)
        var isContinue = _doorsSwitcher;
        var initialLeftPos = _leftDoor.localPosition;
        var initialRightPos = _rightDoor.localPosition;
        float timer = 0;

        while (timer < curve.keys[curve.length - 1].time && (isContinue == _doorsSwitcher))
        {
            float t = curve.Evaluate(timer);
            _leftDoor.localPosition = (1 - t) * initialLeftPos + t * targetLeft;
            _rightDoor.localPosition = (1 - t) * initialRightPos + t * targetRight;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
