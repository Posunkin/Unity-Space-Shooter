using UnityEngine;
using SpaceShooter.Weapons;

public class Blaster : Weapon
{
    internal override void TempFire()
    {
        if (lastShootTime + delayBetweenShots > Time.time) return;
        lastShootTime = Time.time;
        GameObject projGo = Shoot();
        Rigidbody rb = projGo.GetComponent<Rigidbody>();
        rb.velocity = Vector3.up * projectileSpeed;
    }

    internal override GameObject Shoot()
    {
        GameObject projGo = Instantiate(projectilePrefab);
        projGo.transform.position = transform.position;
        return projGo;
    }
}