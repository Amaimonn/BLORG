public class ColourSwapCallback : PriorityCallback
{
    public readonly int ColourNum;
    public ColourSwapCallback(int colourNum, int priority=0, object callback=null) : base(priority, callback)
    {
        ColourNum = colourNum;
    }

}
