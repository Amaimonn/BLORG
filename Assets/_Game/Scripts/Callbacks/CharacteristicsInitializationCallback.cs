public class CharacteristicsInitializationCallback : PriorityCallback
{
    public readonly CharacteristicsController Controller;
    public CharacteristicsInitializationCallback(CharacteristicsController controller, int priority=0, 
        object callback=null) : base(priority, callback)
    {
        Controller = controller;
    }
}
