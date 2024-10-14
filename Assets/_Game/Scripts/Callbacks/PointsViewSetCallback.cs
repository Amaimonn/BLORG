public class PointsViewSetCallback : PriorityCallback
{
    public readonly PointsView View;

    public PointsViewSetCallback(PointsView view, int priority=0, object callback=null) : base(priority, callback)
    {
        View = view;
    }
}
