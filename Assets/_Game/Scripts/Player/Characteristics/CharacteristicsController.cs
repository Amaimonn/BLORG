using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacteristicsController : MonoBehaviour, IStable, IDamageable
{
    [SerializeField] private Aiming _aiming;
    private CharacteristicsView _view;
    private readonly CharacteristicsModel _model = new();
    private bool IsDead => _model.IsDead;
    private Action _fillBlueBar;
    private Action _fillOrangeBar;
    private Action _fillGreenBar;
    private Action _fillHealthBar;

    public void SetView(CharacteristicsView view)
    {    
        _view = view;
        SubscribeIndicators();
        _view.SetBlueFill(_model.EnergyFill[0].CurrentValue / 100.0f);
        _view.SetOrangeFill(_model.EnergyFill[1].CurrentValue / 100.0f);
        _view.SetGreenFill(_model.EnergyFill[2].CurrentValue / 100.0f);
        _view.SetHealthFill(_model.Health.CurrentValue / 100.0f);
        _view.ColourSwapHandler(new ColourSwapCallback(CurrentColourData.CurrentColour));
        ServiceLocator.Current.Get<EventBus>().Subscribe<ShootingToggleCallback>(_view.ShootingToggleHandler);

        if (ServiceLocator.Current.Get<PlayerDataManager>().PlayerObject.TryGetComponent(out PlayerController controller))
        {
            ServiceLocator.Current.Get<EventBus>().Subscribe<ColourSwapCallback>(_view.ColourSwapHandler);
        }
    }

    public bool TryCharge(int colourIndex, float amount)
    {   
        if(_model.EnergyFill[colourIndex].IsFull)
        {
            return false;
        }
        else
        {
            _model.EnergyFill[colourIndex].CurrentValue += amount;
            return true;
        } 
    }

    private void OnEnable()
    {
        _aiming.CurrentEnergyIndicators = _model.EnergyFill;
        _model.Health.Refresh();

        foreach (var bar in _model.EnergyFill)
            bar.Refresh();

        ServiceLocator.Current.Get<EventBus>().Subscribe<CharacteristicsViewSetCallback>(ViewSetHandler);
        ServiceLocator.Current.Get<EventBus>().Invoke(new CharacteristicsInitializationCallback(this));
    }

    private void OnDisable()
    {
        if(_view != null)
        {
            UnsubscribeIndicators();
            ServiceLocator.Current.Get<EventBus>().Unsubscribe<ShootingToggleCallback>(_view.ShootingToggleHandler);
            ServiceLocator.Current.Get<EventBus>().Unsubscribe<ColourSwapCallback>(_view.ColourSwapHandler);
        }

        ServiceLocator.Current.Get<EventBus>().Unsubscribe<CharacteristicsViewSetCallback>(ViewSetHandler);
    }

    private void OnDestroy()
    {
        if(!_model.IsDead)
            SaveData();
    }

    private void ViewSetHandler(CharacteristicsViewSetCallback callback)
    {
        SetView(callback.View);
    }
    
    private void SubscribeIndicators()
    {
        _fillBlueBar = () => _view.SetBlueFill(_model.EnergyFill[0].CurrentValue / 100.0f);
        _fillOrangeBar = () => _view.SetOrangeFill(_model.EnergyFill[1].CurrentValue / 100.0f);
        _fillGreenBar = ()  => _view.SetGreenFill(_model.EnergyFill[2].CurrentValue / 100.0f);

        _fillHealthBar = () => _view.SetHealthFill(_model.Health.CurrentValue / 100.0f);

        _model.EnergyFill[0].OnIndicatorChanged += _fillBlueBar;
        _model.EnergyFill[1].OnIndicatorChanged += _fillOrangeBar;
        _model.EnergyFill[2].OnIndicatorChanged += _fillGreenBar;

        _model.Health.OnIndicatorChanged += _fillHealthBar;
    }

    private void UnsubscribeIndicators()
    {
        _model.EnergyFill[0].OnIndicatorChanged -= _fillBlueBar;
        _model.EnergyFill[1].OnIndicatorChanged -= _fillOrangeBar;
        _model.EnergyFill[2].OnIndicatorChanged -= _fillGreenBar;

        _model.Health.OnIndicatorChanged -= _fillHealthBar;
    }

#region IStable
    public void SaveData()
    {
        var data = new Dictionary<string, object>
        {
            {"Health", _model.Health.CurrentValue},
            {"EnergyList", new List<float>
            {
                _model.EnergyFill[0].CurrentValue,
                _model.EnergyFill[1].CurrentValue,
                _model.EnergyFill[2].CurrentValue
            }}
        };
        var SavedObjectData = new LoadableParameters(data, LoadData);

        StableObjectsData.StableObjectsList.Add(SavedObjectData);
    }

    public void LoadData(LoadableParameters data)
    {
        var parameters  = data.Parameters;

        if (ServiceLocator.Current.Get<PlayerDataManager>().PlayerObject
            .TryGetComponent(out CharacteristicsController controller))
        {
            controller._model.Health.CurrentValue = (int)parameters["Health"];
            for (var iter = 0; iter < _model.EnergyFill.Count; iter++)
            {
                controller._model.EnergyFill[iter].CurrentValue = ((List<float>)parameters["EnergyList"])[iter];
            }
        }
        else
        {
            Debug.LogWarning("No controller found to load characteristics");
        }
    }
#endregion

#region IDamagable
    public void TakeDamage(int damage)
    {
        Debug.Log($"damage: {damage}");
        _model.Health.CurrentValue -= damage;
        if (IsDead)
            Death();
    }
#endregion

    private void Death()
    {
        ServiceLocator.Current.Get<EventBus>().Invoke(new ShootingToggleCallback(false));
        gameObject.SetActive(false);
        ServiceLocator.Current.Get<EventBus>().Invoke(new GameOverCallback());
    }
}
