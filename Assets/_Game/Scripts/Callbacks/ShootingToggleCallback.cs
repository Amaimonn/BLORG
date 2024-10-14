public class ShootingToggleCallback : PriorityCallback
{
    public readonly bool IsShooting;
    public ShootingToggleCallback(bool isShooting, int priority=0, object callback=null) : base(priority, callback)
    {
        IsShooting = isShooting;
    }
}
