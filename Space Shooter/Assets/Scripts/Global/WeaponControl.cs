using System;
using UnityEngine;

public class WeaponControl : MonoBehaviour
{
    public Action OnWeaponShoot;

    private void Update()
    {
        OnWeaponShoot?.Invoke();
    }
}
