using SpaceShooter.Weapons;
using UnityEngine;

public class PlayerWeaponControl : WeaponControl
{
    private WeaponType firstWeaponType;
    private GameObject[] currentWeapons = new GameObject[2];

    private void Awake()
    {
        GameObject weapon = weaponManager.SetWeapon(WeaponType.blaster);
        currentWeapons[0] = SetWeaponPosition(weapon, 0);
        firstWeaponType = weapon.GetComponent<Weapon>().type;
    }
}
