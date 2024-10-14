public class PointsInitializationCallback : PriorityCallback
{
    public readonly PointsController Controller;

    public PointsInitializationCallback(PointsController controller, int priority=0, 
        object callback=null) : base(priority, callback)
    {
        Controller = controller;
    }
}
