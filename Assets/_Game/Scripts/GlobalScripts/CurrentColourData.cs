public static class CurrentColourData
{
    public static int currentColour = 0;
    private const int DEFAULT_COLOUR_INDEX = 0;
    public static int CurrentColour
    {
        get { return currentColour; }
        set { currentColour = value; }
    }
    public static void SetDefaultColour()
    {
        currentColour = DEFAULT_COLOUR_INDEX;
    }
}
