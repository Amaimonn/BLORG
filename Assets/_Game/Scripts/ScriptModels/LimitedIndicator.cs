using System;
using UnityEngine;

[Serializable]
public class LimitedIndicator<T> where T: IComparable
{
    public event Action OnIndicatorChanged;
    public T CurrentValue
    {
        get => _currentValue;
        set
        {
            if (_topLimit.CompareTo(value) < 0)
                _currentValue = _topLimit;
            else if (_bottomLimit.CompareTo(value) > 0)
                _currentValue = _bottomLimit;
            else
                _currentValue = value;
            OnIndicatorChanged?.Invoke();
        }
    }

    public bool IsFull => _currentValue.Equals(_topLimit);
    public bool IsEmpty() => _currentValue.Equals(_bottomLimit);

    [SerializeField] private T _currentValue;
    [SerializeField] private T _bottomLimit;
    [SerializeField] private T _topLimit;

    public LimitedIndicator(T bottomLimit, T topLimit, T initialValue)
    {
        _bottomLimit = bottomLimit;
        _topLimit = topLimit;
        if (bottomLimit.CompareTo(initialValue) <= 0 && topLimit.CompareTo(CurrentValue) >= 0)
            _currentValue = initialValue;
        else
            _currentValue = bottomLimit;
    }
    
    public void Refresh()
    {
        CurrentValue = _topLimit;
    }
}
