using UnityEngine;

public class MagicShot : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float launchVelocity = 700f;
    [SerializeField] private float shotDelay = 1f;

    void Update()
    {
        if (shotDelay > 0)
            shotDelay -= Time.deltaTime;
        if (Input.GetMouseButton(0) && shotDelay <= 0)
        {
            shotDelay = 1.0f;
            GameObject ball = Instantiate(projectile, transform.position, transform.rotation);
            ball.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0.0f, 0.0f, launchVelocity));
        }
    }
}