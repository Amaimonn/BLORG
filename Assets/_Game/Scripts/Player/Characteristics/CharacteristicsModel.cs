using System.Collections.Generic;

public class CharacteristicsModel
{
    public List<LimitedIndicator<float>> EnergyFill { get; } = new() 
    {
        new LimitedIndicator<float>(0.0f, 100.0f, 50.0f),
        new LimitedIndicator<float>(0.0f, 100.0f, 50.0f),
        new LimitedIndicator<float>(0.0f, 100.0f, 50.0f)
    };

    public LimitedIndicator<int> Health{ get;  }  = new(0, 100, 100);

    public bool IsDead {get => Health.CurrentValue  <=  0; }
}
