using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightColour : MonoBehaviour
{
    [SerializeField] private Light _mainLight;
    [SerializeField] private ColourDataSO _colourData;
    // private static bool valueReseted = false;

    private void Start()
    {
        // if (!valueReseted)
        // {
        //     CurrentColourData.SetDefaultColour();
        //     valueReseted = true;
        // }
        
        _mainLight.color = _colourData.MagicColour[CurrentColourData.CurrentColour];

        // if (ServiceLocator.Current.Get<PlayerDataManager>().PlayerObject && ServiceLocator.Current.Get<PlayerDataManager>().PlayerObject.TryGetComponent(out PlayerController controller))
        // {
        //     // controller.OnColourChange += SwapLightColour;
        //     ServiceLocator.Current.Get<EventBus>().Subscribe<ColourSwapCallback>(SwapLightColour);
        // }
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
