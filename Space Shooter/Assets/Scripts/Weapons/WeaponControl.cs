using System;
using SpaceShooter.Weapons;
using UnityEngine;

public abstract class WeaponControl : MonoBehaviour
{
    public Action OnWeaponShoot;
    public Action OnSpecialWeaponShot;
    [SerializeField] protected Transform[] weaponSlots;
    [SerializeField] protected WeaponManager weaponManager = WeaponManager.S;

    protected virtual void Update()
    {
        OnWeaponShoot?.Invoke();
    }

    protected GameObject SetWeaponPosition(GameObject weaponToSet, Transform slot)
    {
        GameObject weapon = Instantiate(weaponToSet, slot.position, slot.rotation, slot.transform);
        return weapon;
    }
    
}
