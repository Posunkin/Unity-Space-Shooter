using System;
using UnityEngine;

public abstract class WeaponControl : MonoBehaviour
{
    public Action OnWeaponShoot;

    internal void Update()
    {
        OnWeaponShoot?.Invoke();
    }
    
}
