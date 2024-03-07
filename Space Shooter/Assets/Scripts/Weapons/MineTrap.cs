using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class MineTrap : Weapon
    {
        [Header("Mine definition")]
        [SerializeField] private float _timeToExplose;
        [SerializeField] private float _explosionRadius;
        internal float timeToExplose 
        { 
            get => _timeToExplose; 
            set => _timeToExplose = value > 0 ? value : 1.5f;
        }
        internal float explosionRadius
        {
            get => _explosionRadius;
            set => _explosionRadius = value > 0 ? value : 5f;
        }

        protected override void Start()
        {
            type = WeaponType.mineTrap;
            if (isPlayer) weaponControl.OnSpecialWeaponShot += TempFire;
            else weaponControl.OnWeaponShoot += TempFire;        
        }

        protected override void TempFire()
        {
            if (lastShootTime + delayBetweenShots > Time.time) return;
            lastShootTime = Time.time;
            GameObject projGo = Shoot();
        }

        protected override void OnDisable()
        {
            weaponControl.OnSpecialWeaponShot -= TempFire;
            base.OnDisable();
        }

        protected override GameObject Shoot()
        {
            GameObject projGo = (GameObject)Instantiate(currentProjectile, projectileAnchor);
            projGo.GetComponent<MineProjectile>().SetProjectile(currentDamage, _timeToExplose, _explosionRadius, isPlayer);
            projGo.transform.position = transform.position;
            return projGo;
        }
    }
}

