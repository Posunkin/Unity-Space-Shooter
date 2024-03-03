using UnityEngine;
using SpaceShooter.Weapons;
using SpaceShooter.Enemies;

public class Blaster : Weapon
{
    private void Awake()
    {
        type = WeaponType.blaster;
        parent = this.gameObject.transform.root;
        if (parent.GetComponent<Enemy>() == null) isPlayer = true;
        else isPlayer = false;
    }

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
        GameObject projGo;
        if (isPlayer) 
        {
            projGo = Instantiate(projectilePrefab);
        }
        else 
        {
            projGo = Instantiate(enemyProjectilePrefab);
        }
        projGo.GetComponent<Projectile>().SetProjectile(currentDamage);
        projGo.transform.position = transform.position;
        return projGo;
    }
}