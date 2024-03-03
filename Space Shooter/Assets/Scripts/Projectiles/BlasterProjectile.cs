using UnityEngine;

public class BlasterProjectile : Projectile
{
    internal override void OnCollisionEnter()
    {
        Destroy(gameObject);
    }
}
