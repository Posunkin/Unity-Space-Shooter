using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class Blaster : Weapon
    {
        private Vector3 projectileDirection;

        protected override void Start()
        {
            type = WeaponType.blaster;
            projectileDirection = isPlayer ? Vector3.up : Vector3.down;
            weaponControl.OnWeaponShoot += TempFire;
            if (isPlayer) pitchRange = new Vector2(0.85f, 1.2f);
            else pitchRange = new Vector2(0.6f, 0.9f);
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
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            audioSource.PlayOneShot(shotSound);
            GameObject projGo = (GameObject) Instantiate(currentProjectile, projectileAnchor);
            projGo.GetComponent<Projectile>().SetProjectile(currentDamage);
            projGo.transform.position = transform.position;
            return projGo;
        }
    }
}