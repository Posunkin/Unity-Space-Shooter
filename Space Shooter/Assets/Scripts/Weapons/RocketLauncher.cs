using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class RocketLauncher : Weapon
    {
        protected override void Start()
        {
            type = WeaponType.rocketLauncher;
            if (isPlayer) weaponControl.OnSpecialWeaponShot += TempFire;
            else weaponControl.OnWeaponShoot += TempFire;
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
            projGo.GetComponent<RocketProjectile>().SetProjectile(currentDamage, projectileSpeed);
            projGo.transform.position = transform.position;
            return projGo;
        }
    }
}   
