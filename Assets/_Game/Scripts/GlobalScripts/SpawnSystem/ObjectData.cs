using System;
using System.Collections.Generic;

[Serializable]
public class LoadableParameters
{
    public Dictionary<string, object> Parameters { get => _parameters; }
    public Action<LoadableParameters> OnLoadParameters  { get => _loadParameters; }

    private readonly Dictionary<string, object> _parameters;
    private readonly Action<LoadableParameters> _loadParameters;

    public LoadableParameters(Dictionary<string, object> parameters, Action<LoadableParameters> action)
    {
        _parameters = parameters;
        _loadParameters = action;
    }
}
