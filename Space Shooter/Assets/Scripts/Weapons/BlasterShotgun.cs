using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class BlasterShotgun : Weapon
    {
        public int projectilesNum = 3;
        public float projectileAngle = 5f;
        private Vector3 projectileDirection;

        protected override void Start()
        {
            type = WeaponType.blasterShotgun;
            projectileDirection = isPlayer ? Vector3.up : Vector3.down;
            weaponControl.OnWeaponShoot += TempFire;
            if (isPlayer) pitchRange = new Vector2(0.85f, 1.2f);
            else pitchRange = new Vector2(0.6f, 0.9f);
        }

        protected override void TempFire()
        {
            if (lastShootTime + delayBetweenShots > Time.time) return;
            lastShootTime = Time.time;
            if (projectilesNum % 2 == 1)
            {
                GameObject projGo = Shoot();
                Rigidbody rb = projGo.GetComponent<Rigidbody>();
                rb.velocity =  projectileDirection * projectileSpeed;
                int proj = projectilesNum / 2;
                for (int i = 1; i <= proj; i++)
                {
                    projGo = Shoot();
                    rb = projGo.GetComponent<Rigidbody>();
                    projGo.transform.rotation = Quaternion.AngleAxis(projectileAngle * i, Vector3.back);
                    rb.velocity =  projGo.transform.rotation * projectileDirection * projectileSpeed;
                }
                for (int i = 1; i <= proj; i++)
                {
                    projGo = Shoot();
                    rb = projGo.GetComponent<Rigidbody>();
                    projGo.transform.rotation = Quaternion.AngleAxis(-projectileAngle * i, Vector3.back);
                    rb.velocity =  projGo.transform.rotation * projectileDirection * projectileSpeed;
                }
            }
            audioSource.pitch = Random.Range(pitchRange.x, pitchRange.y);
            audioSource.PlayOneShot(shotSound);
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

