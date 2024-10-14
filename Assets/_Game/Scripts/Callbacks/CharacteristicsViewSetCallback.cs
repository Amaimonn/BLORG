public class CharacteristicsViewSetCallback : PriorityCallback
{
    public readonly CharacteristicsView View;
    public CharacteristicsViewSetCallback(CharacteristicsView view, int priority=0, object callback=null) : base(priority, callback)
    {
        View = view;
    }
}