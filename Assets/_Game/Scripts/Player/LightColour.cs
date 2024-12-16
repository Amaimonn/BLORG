using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightColour : MonoBehaviour
{
    [SerializeField] private Light _mainLight;
    [SerializeField] private ColourDataSO _colourData;

    private void Start()
    {
        _mainLight.color = _colourData.MagicColour[CurrentColourData.CurrentColour];
    }

    private void OnEnable()
    {
        ServiceLocator.Current.Get<EventBus>().Subscribe<ColourSwapCallback>(SwapLightColour);
    }

    private void OnDisable()
    {
        ServiceLocator.Current.Get<EventBus>().Unsubscribe<ColourSwapCallback>(SwapLightColour);
    }

    private void SwapLightColour(ColourSwapCallback callback)
    {
        _mainLight.color = _colourData.MagicColour[callback.ColourNum];
    }
}
