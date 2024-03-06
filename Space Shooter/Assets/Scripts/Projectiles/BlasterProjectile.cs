using SpaceShooter.Enemies;
using UnityEngine;

public class BlasterProjectile : Projectile
{
    protected override void OnCollisionEnter(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null) enemy.TakeDamage(damageToDeal);
        Destroy(gameObject);
    }

    protected override void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
