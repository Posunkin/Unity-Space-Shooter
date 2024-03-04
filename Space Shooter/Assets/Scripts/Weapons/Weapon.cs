using System;
using SpaceShooter.Enemies;
using UnityEngine;

namespace SpaceShooter.Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        [SerializeField] protected GameObject playerProjectilePrefab;
        [SerializeField] protected GameObject enemyProjectilePrefab;
        [SerializeField] private float _currentDamage;
        [SerializeField] private float _projectileSpeed;
        [SerializeField] protected float _delayBetweenShots;
        [SerializeField] internal WeaponType type;
        [SerializeField] protected bool isPlayer;
        
        protected Transform projectileAnchor;
        protected Transform parent;
        protected float lastShootTime;
        protected WeaponControl weaponControl;
        protected GameObject currentProjectile;

        internal float delayBetweenShots { get => _delayBetweenShots; set => _delayBetweenShots = value;}
        internal float projectileSpeed { get => _projectileSpeed; set => _projectileSpeed = value; }
        internal float currentDamage { get => _currentDamage; set => _currentDamage = value; }

        protected void Awake()
        {
            weaponControl = GetComponentInParent<WeaponControl>();
            weaponControl.OnWeaponShoot += TempFire;
            parent = this.gameObject.transform.root;
            projectileAnchor = GameObject.Find("PROJECTILE ANCHOR").transform;
            if (parent.tag == "Player") 
            {
                isPlayer = true;
                currentProjectile = playerProjectilePrefab;
            }
            else 
            {
                isPlayer = false;
                currentProjectile = enemyProjectilePrefab;
            }
        }
        
        protected abstract void TempFire();
        protected abstract GameObject Shoot();
    }
}

