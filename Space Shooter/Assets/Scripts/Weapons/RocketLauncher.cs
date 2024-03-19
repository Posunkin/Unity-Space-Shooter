using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class RocketLauncher : Weapon
    {
        protected override void Start()
        {
            type = WeaponType.rocketLauncher;
            if (isPlayer) 
            {
                weaponControl.OnSpecialWeaponShot += TempFire;
                currentProjectile = playerProjectilePrefab;
            }
            else 
            {
                weaponControl.OnWeaponShoot += TempFire;
                currentProjectile = enemyProjectilePrefab;
            }
        }

        protected override void OnDisable()
        {
            weaponControl.OnSpecialWeaponShot -= TempFire;
            base.OnDisable();
        }

        protected override void TempFire()
        {
            if (lastShootTime + delayBetweenShots > Time.time) return;
            lastShootTime = Time.time;
            GameObject projGo = Shoot();
        }

        protected override GameObject Shoot()
        {
            GameObject projGo = (GameObject) Instantiate(currentProjectile, projectileAnchor);
            projGo.GetComponent<RocketProjectile>().SetProjectile(currentDamage, projectileSpeed, isPlayer);
            projGo.transform.position = transform.position;
            if (!isPlayer) projGo.transform.rotation = Quaternion.Euler(90, 0, 0);
            return projGo;
        }
    }
}   
