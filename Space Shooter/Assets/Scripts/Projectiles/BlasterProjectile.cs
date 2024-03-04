using UnityEngine;

public class BlasterProjectile : Projectile
{
    protected override void OnCollisionEnter()
    {
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
