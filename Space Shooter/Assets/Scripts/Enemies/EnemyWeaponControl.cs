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
        weaponManager = WeaponManager.S;
        GameObject weapon = weaponManager.GetWeapon(type);
        weapon = SetWeaponPosition(weapon, weaponSlots[0]);
        Weapon currentWeapon = weapon.GetComponent<Weapon>();
        currentWeapon.delayBetweenShots = enemyDelayBetweenShots;
        currentWeapon.projectileSpeed = enemyProjectileSpeed;
    }
}
