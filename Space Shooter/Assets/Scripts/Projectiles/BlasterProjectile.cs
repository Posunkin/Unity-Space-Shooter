using SpaceShooter.Enemies;
using UnityEngine;

public class BlasterProjectile : Projectile
{
    protected void OnCollisionEnter(Collision other)
    {
        Enemy enemy = other.gameObject.GetComponent<Enemy>();
        if (enemy != null) enemy.TakeDamage(damageToDeal);
        Destroy(gameObject);
    }

    protected void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
