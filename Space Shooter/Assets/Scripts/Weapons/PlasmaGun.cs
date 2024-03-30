using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class PlasmaGun : Weapon
    {
        private Vector3 projectileDirection;

        protected override void Start()
        {
            type = WeaponType.plasmaGun;
            projectileDirection = isPlayer ? Vector3.up : Vector3.down;
            weaponControl.OnWeaponShoot += TempFire;
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
            audioSource.PlayOneShot(shotSound);
            GameObject projGo = (GameObject) Instantiate(currentProjectile, projectileAnchor);
            projGo.GetComponent<Projectile>().SetProjectile(currentDamage);
            projGo.transform.position = transform.position;
            return projGo;
        }
    }
}