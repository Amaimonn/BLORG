using System;

public class PointsModel
{
    public int Points {get => _points; set => _points = value; }
    public event Action<int> OnPointsAdded;
    public event Action<int> OnPointsSet;
    private int _points = 0;

    public void AddPoints(int count)
    {
        _points += count;
        OnPointsAdded?.Invoke(_points);
    }

    public void SetPoints(int count)
    {
        _points = count;
        OnPointsSet?.Invoke(_points);
    }
}