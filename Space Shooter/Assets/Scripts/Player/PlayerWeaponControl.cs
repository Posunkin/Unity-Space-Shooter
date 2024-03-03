using SpaceShooter.Weapons;
using UnityEngine;

public class PlayerWeaponControl : WeaponControl
{
    [SerializeField] private Transform[] weaponSlots;
    [SerializeField] private WeaponManager weaponManager;
    private WeaponType firstWeaponType;

    private void Awake()
    {
        GameObject weapon = weaponManager.SetWeapon(WeaponType.blaster);
        SetWeaponPosition(weapon, 0);
        firstWeaponType = weapon.GetComponent<Weapon>().type;
    }

    private void SetWeaponPosition(GameObject weaponToSet, int slot)
    {
        GameObject weapon = Instantiate(weaponToSet, weaponSlots[slot].position, weaponSlots[slot].rotation, this.gameObject.transform);
    }
}
