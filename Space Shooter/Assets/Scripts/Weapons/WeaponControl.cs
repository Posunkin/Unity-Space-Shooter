using System;
using SpaceShooter.Weapons;
using UnityEngine;

public abstract class WeaponControl : MonoBehaviour
{
    public Action OnWeaponShoot;
    [SerializeField] protected Transform[] weaponSlots;
    [SerializeField] protected WeaponManager weaponManager = WeaponManager.S;

    protected void Update()
    {
        OnWeaponShoot?.Invoke();
    }

    protected GameObject SetWeaponPosition(GameObject weaponToSet, int slot)
    {
        GameObject weapon = Instantiate(weaponToSet, weaponSlots[slot].position, weaponSlots[slot].rotation, weaponSlots[slot].transform);
        return weapon;
    }
    
}
