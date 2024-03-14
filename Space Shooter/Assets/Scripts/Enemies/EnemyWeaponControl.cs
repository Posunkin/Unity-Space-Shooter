using SpaceShooter.Weapons;
using UnityEngine;

public class EnemyWeaponControl : WeaponControl
{
    [SerializeField] private WeaponType[] type;
    [SerializeField] private float enemyDelayBetweenShots;
    [SerializeField] private float enemyProjectileSpeed;
    [SerializeField] private float explosionRadius;
    [SerializeField] private float timeToExplose;
    private GameObject weapon;

    private void Awake()
    {
        weaponManager = WeaponManager.Instance;
        for (int i = 0; i < type.Length; i++)
        {
            GameObject weapon = weaponManager.GetWeapon(type[i]);
            weapon = SetWeaponPosition(weapon, weaponSlots[i]);
            Weapon currentWeapon = weapon.GetComponent<Weapon>();
            currentWeapon.delayBetweenShots = enemyDelayBetweenShots;
            currentWeapon.projectileSpeed = enemyProjectileSpeed;
            if (currentWeapon.type == WeaponType.mineTrap)
            {
                currentWeapon.GetComponent<MineTrap>().explosionRadius = explosionRadius;
                currentWeapon.GetComponent<MineTrap>().timeToExplose = timeToExplose;
            }
        }
       
    }
}
