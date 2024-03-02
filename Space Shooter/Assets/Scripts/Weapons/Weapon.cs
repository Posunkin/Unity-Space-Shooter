using System;
using UnityEngine;

namespace SpaceShooter.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] internal GameObject projectilePrefab;
        [SerializeField] internal float projectileSpeed;
        [SerializeField] internal float delayBetweenShots;
        internal float lastShootTime;
        internal WeaponControl weaponControl;

        internal void Start()
        {
            weaponControl = GetComponentInParent<WeaponControl>();
            weaponControl.OnWeaponShoot += TempFire;
        }

        internal abstract void TempFire();
        internal abstract GameObject Shoot();
    }
}

