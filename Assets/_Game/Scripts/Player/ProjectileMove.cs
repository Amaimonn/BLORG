using UnityEngine;

[RequireComponent (typeof(Rigidbody), typeof(Light))]
public class ProjectileMove : MonoBehaviour
{
    
    [SerializeField] private float _bulletSpeed=100;
    [SerializeField] private int _damage=100;
    [SerializeField] private ColourDataSO _colourData;
    [SerializeField] private Light _light;
    [SerializeField] private Rigidbody _rigidbody;

    private int _magicColour;

    private void Start()
    {
        _rigidbody.linearVelocity = transform.forward * _bulletSpeed;
        _magicColour = CurrentColourData.CurrentColour;
        _light.color = _colourData.MagicColour[CurrentColourData.CurrentColour];
        Physics.IgnoreCollision(ServiceLocator.Current.Get<PlayerDataManager>()
            .PlayerObject.GetComponent<Collider>(), GetComponent<Collider>());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out DamageableEnemy enemy))
        {
            enemy.TakeColourDamage(_damage, _magicColour);
        }

        Destroy(gameObject);
    }

}