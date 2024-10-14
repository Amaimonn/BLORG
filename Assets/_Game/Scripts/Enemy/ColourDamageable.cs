using UnityEngine;
public abstract class ColourDamageable : MonoBehaviour, IDamageable
{
    [Header("Characteristic")]
    [SerializeField] protected LimitedIndicator<int> _health = new(0, 100, 100);
    [SerializeField] protected int _colourIndex;

    public abstract void TakeColourDamage(int damage, int colourIndex);

    public virtual void TakeDamage(int damage)
    {
        _health.CurrentValue -= damage;
    }
}
