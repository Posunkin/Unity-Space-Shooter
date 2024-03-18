using System;
using UnityEngine;

public abstract class WeaponControl : MonoBehaviour
{
    public Action OnWeaponShoot;
    public Action OnSpecialWeaponShot;
    [SerializeField] protected Transform[] weaponSlots;
    [SerializeField] protected WeaponManager weaponManager = WeaponManager.Instance;
    private bool isStoped = false;

    protected virtual void Update()
    {
        if (!isStoped) OnWeaponShoot?.Invoke();
    }

    protected GameObject SetWeaponPosition(GameObject weaponToSet, Transform slot)
    {
        GameObject weapon = Instantiate(weaponToSet, slot.position, slot.rotation, slot.transform);
        return weapon;
    }

    public void Stop()
    {
        isStoped = true;
    }

    public void Activate()
    {
        isStoped = false;
    }
    
}
