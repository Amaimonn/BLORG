using System.Collections.Generic;
using UnityEngine;

public class PointsController : MonoBehaviour, IStable
{   
    public int Points {get => _pointsModel.Points;}
    private readonly PointsModel _pointsModel = new();
    private PointsView  _pointsView;

    public void SetView(PointsView view)
    {
        _pointsView = view;
        _pointsView.SetPointsText(_pointsModel.Points);
        _pointsModel.OnPointsAdded += _pointsView.SetPointsText;
        _pointsModel.OnPointsSet  +=  _pointsView.SetPointsText;
    }

    public void AddPoints(int count)
    {
        _pointsModel.AddPoints(count);
    }

    private void Start()
    {
        ServiceLocator.Current.Get<EventBus>().Invoke(new PointsInitializationCallback(this));
    }
    
    private void OnEnable()
    {
        ServiceLocator.Current.Get<EventBus>().Subscribe<PointsViewSetCallback>(ViewSetHandler);
    }
    
    private void OnDisable()
    {
        if(_pointsView != null)
        {
            _pointsModel.OnPointsAdded -= _pointsView.SetPointsText;
            _pointsModel.OnPointsSet  -=  _pointsView.SetPointsText;
        }

        ServiceLocator.Current.Get<EventBus>().Unsubscribe<PointsViewSetCallback>(ViewSetHandler);
    }

    private void ViewSetHandler(PointsViewSetCallback callback)
    {
        SetView(callback.View);
    }

    private void OnDestroy()
    {
        SaveData();
    }

#region IStable
    public void SaveData()
    {
        var data = new Dictionary<string, object>
        {
            {"Points", _pointsModel.Points}
        };
        var SavedObjectData = new LoadableParameters(data, LoadData);

        StableObjectsData.StableObjectsList.Add(SavedObjectData);
    }

    public void LoadData(LoadableParameters data)
    {
        var controller = ServiceLocator.Current.Get<PlayerDataManager>().PlayerObject.transform
            .GetComponentInChildren<PointsController>();
            
        if (controller != null)
            controller._pointsModel.SetPoints((int)data.Parameters["Points"]);
        else
            Debug.LogWarning("No PointsController found to load points data");
    }
#endregion
}
