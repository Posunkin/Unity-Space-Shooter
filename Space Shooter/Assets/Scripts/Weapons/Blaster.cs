using UnityEngine;
using SpaceShooter.Weapons;

public class Blaster : Weapon
{
    private Vector3 projectileDirection;

    private void Start()
    {
        type = WeaponType.blaster;
        projectileDirection = isPlayer ? Vector3.up : Vector3.down;
    }

    protected override void TempFire()
    {
        if (lastShootTime + delayBetweenShots > Time.time) return;
        lastShootTime = Time.time;
        GameObject projGo = Shoot();
        Rigidbody rb = projGo.GetComponent<Rigidbody>();
        rb.velocity =  projectileDirection * projectileSpeed;
    }

    protected override GameObject Shoot()
    {
        GameObject projGo = (GameObject) Instantiate(currentProjectile, projectileAnchor);
        projGo.GetComponent<Projectile>().SetProjectile(currentDamage);
        projGo.transform.position = transform.position;
        return projGo;
    }
}