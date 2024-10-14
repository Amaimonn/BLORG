using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacteristicsView : MonoBehaviour
{
    [SerializeField] private Image _blueEnergyBar;
    [SerializeField] private Image _orangeEnergyBar;
    [SerializeField] private Image _greenEnergyBar;
    [SerializeField] private Image _atackState;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _aimImage;
    [SerializeField] private ColourDataSO _colourData;

    private void OnEnable()
    {
        _aimImage.gameObject.SetActive(false);
        ServiceLocator.Current.Get<EventBus>().Invoke(new CharacteristicsViewSetCallback(this));
        ServiceLocator.Current.Get<EventBus>().Subscribe<CharacteristicsInitializationCallback>(CharacteristicsInitializationHandler);
    }

    private void OnDisable()
    {
        ServiceLocator.Current.Get<EventBus>().Unsubscribe<CharacteristicsInitializationCallback>(CharacteristicsInitializationHandler);
    }

    public void SetBlueFill(float amount) => SetFillAmount(_blueEnergyBar, amount);
    public void SetOrangeFill(float amount) => SetFillAmount(_orangeEnergyBar, amount);
    public void SetGreenFill(float amount) => SetFillAmount(_greenEnergyBar, amount);
    public void SetHealthFill(float amount)  => SetFillAmount(_healthBar, amount);

    public void ColourSwapHandler(ColourSwapCallback callback)
    {
        _atackState.color = _colourData.MagicColour[callback.ColourNum];
    }

    public void ShootingToggleHandler(ShootingToggleCallback callback)
    {
        if(callback.IsShooting)
            {
                _atackState.rectTransform.Rotate(new Vector3(0.0f, 0.0f, 45.0f));
                _aimImage.gameObject.SetActive(true);
            }
        else
            {
                _atackState.rectTransform.Rotate(new Vector3(0.0f,  0.0f, -45.0f));
                _aimImage.gameObject.SetActive(false);
            }
    }

    private void SetFillAmount(Image bar, float amount)
    {
        bar.fillAmount = amount;
    }

    private void CharacteristicsInitializationHandler(CharacteristicsInitializationCallback callback)
    {
        callback.Controller.SetView(this);
    }
}
