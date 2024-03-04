using SpaceShooter.Weapons;
using UnityEngine;

public class EnemyWeaponControl : WeaponControl
{
    [SerializeField] private WeaponType type;
    [SerializeField] private float enemyDelayBetweenShots;
    [SerializeField] private float enemyProjectileSpeed;
    private GameObject weapon;

    private void Awake()
    {
        weaponManager = GameObject.Find("Game Manager").GetComponent<WeaponManager>();
        GameObject weapon = weaponManager.SetWeapon(type);
        weapon = SetWeaponPosition(weapon, 0);
        Weapon currentWeapon = weapon.GetComponent<Weapon>();
        currentWeapon.delayBetweenShots = enemyDelayBetweenShots;
        currentWeapon.projectileSpeed = enemyProjectileSpeed;
    }
}
