using SpaceShooter.Weapons;
using UnityEngine;

public class EnemyWeaponControl : WeaponControl
{
    [SerializeField] private WeaponType[] type;
    [SerializeField] private float[] enemyDelayBetweenShots;
    [SerializeField] private float[] enemyProjectileSpeed;
    [Header("Mine trapp:")]
    [SerializeField] private float explosionRadius;
    [SerializeField] private float timeToExplose;
    [Header("Shotgun:")]
    [SerializeField] private int projectilesNum;
    [SerializeField] private float projectileAngle;
    private GameObject weapon;
    private Weapon[] currentWeapon;

    private void Awake()
    {
        weaponManager = WeaponManager.Instance;
        Init();
    }

    public void Init()
    {
        currentWeapon = new Weapon[type.Length];
        for (int i = 0; i < type.Length; i++)
        {
            GameObject weapon = weaponManager.GetWeapon(type[i]);
            weapon = SetWeaponPosition(weapon, weaponSlots[i]);
            currentWeapon[i] = weapon.GetComponent<Weapon>();
            currentWeapon[i].delayBetweenShots = enemyDelayBetweenShots[i];
            currentWeapon[i].projectileSpeed = enemyProjectileSpeed[i];
            if (currentWeapon[i].type == WeaponType.mineTrap)
            {
                currentWeapon[i].GetComponent<MineTrap>().explosionRadius = explosionRadius;
                currentWeapon[i].GetComponent<MineTrap>().timeToExplose = timeToExplose;
            }
            if (currentWeapon[i].type == WeaponType.blasterShotgun)
            {
                currentWeapon[i].GetComponent<BlasterShotgun>().projectilesNum = projectilesNum;
                currentWeapon[i].GetComponent<BlasterShotgun>().projectileAngle = projectileAngle;
            }
        }
    }
}
