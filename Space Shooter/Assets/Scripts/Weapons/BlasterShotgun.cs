using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class BlasterShotgun : Weapon
    {
        private Vector3 projectileDirection;
        private float xAngle = 5;

        protected override void Start()
        {
            type = WeaponType.blasterShotgun;
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
            projGo = Shoot();
            rb = projGo.GetComponent<Rigidbody>();
            projGo.transform.rotation = Quaternion.AngleAxis(xAngle, Vector3.back);
            rb.velocity = projGo.transform.rotation * projectileDirection * projectileSpeed;
            projGo = Shoot();
            rb = projGo.GetComponent<Rigidbody>();
            projGo.transform.rotation = Quaternion.AngleAxis(-xAngle, Vector3.back);
            rb.velocity = projGo.transform.rotation * projectileDirection * projectileSpeed;
        }

        protected override GameObject Shoot()
        {
            GameObject projGo = (GameObject) Instantiate(currentProjectile, projectileAnchor);
            projGo.GetComponent<BlasterProjectile>().SetProjectile(currentDamage);
            projGo.transform.position = transform.position;
            return projGo;
        }
    }
}

