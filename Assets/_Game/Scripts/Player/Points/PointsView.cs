using TMPro;
using UnityEngine;

public class PointsView : MonoBehaviour
{
    public string PointsText {get => _points.text;}
    [SerializeField] private TMP_Text _points;
    
    public void SetPointsText(int points)
    {
        _points.text = points.ToString();
    }

    private void OnEnable()
    {
        ServiceLocator.Current.Get<EventBus>().Invoke(new PointsViewSetCallback(this));
        ServiceLocator.Current.Get<EventBus>().Subscribe<PointsInitializationCallback>(PointsInitializationHandler);
    }

    private void OnDisable()
    {
        ServiceLocator.Current.Get<EventBus>().Unsubscribe<PointsInitializationCallback>(PointsInitializationHandler);
    }

    private void PointsInitializationHandler(PointsInitializationCallback callback)
    {
        callback.Controller.SetView(this);
    }
}
