using SpaceShooter.Enemies;
using UnityEngine;

public class BlasterProjectile : Projectile
{
    protected void OnCollisionEnter(Collision other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(damageToDeal);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
