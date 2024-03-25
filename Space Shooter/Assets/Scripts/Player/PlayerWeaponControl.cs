using System;
using System.Collections;
using System.Linq;
using SpaceShooter.Weapons;
using UnityEngine;

public class PlayerWeaponControl : WeaponControl
{
    public Action<int> OnChargesSpend;

    [SerializeField] private PlayerStats player;
    [SerializeField] private WeaponType firstWeaponType;
    [SerializeField] private WeaponType specWeaponType;
    [SerializeField] private Transform specSlot;
    [SerializeField] private WeaponType[] specWeapons;
    [SerializeField] private PlayerUI playerUI;
    private GameObject[] currentWeapons = new GameObject[3];
    private Weapon[] currentWeaponType = new Weapon[3];
    private GameObject currentSpecWeap;
    private Weapon currentSpecWeapType;
    [SerializeField] private int _specCharges = 1;
    private bool specWeaponOnCharge = false;
    private float specWeaponChargingTime = 2;

    private float damageMult = 1;

    private void Awake()
    {
        player.OnWeaponAbsorb += SetNewWeapon;
        GameObject weapon = weaponManager.GetWeapon(firstWeaponType);
        currentWeapons[0] = SetWeaponPosition(weapon, weaponSlots[0]);
        currentWeaponType[0] = currentWeapons[0].GetComponent<Weapon>();
        GameObject specWeapon = weaponManager.GetWeapon(specWeaponType);
        currentSpecWeap = SetWeaponPosition(specWeapon, specSlot);
        currentSpecWeapType = currentSpecWeap.GetComponent<Weapon>();
        UpdateCharges(_specCharges);
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space) && _specCharges > 0 && !specWeaponOnCharge)
        {
            OnSpecialWeaponShot?.Invoke();
            UpdateCharges(--_specCharges);
            StartCoroutine(nameof(ChargingWeapon));
        }
    }

    private IEnumerator ChargingWeapon()
    {
        specWeaponOnCharge = true;
        yield return new WaitForSeconds(specWeaponChargingTime);
        specWeaponOnCharge = false;
    }

    private void OnDisable()
    {
        player.OnWeaponAbsorb -= SetNewWeapon;
    }

    private void SetNewWeapon(WeaponType type)
    {
        if (specWeapons.Contains(type))
        {
            SetSpecWeapon(type);
            return;
        }
        GameObject weapon = weaponManager.GetWeapon(type);
        if (type != currentWeapons[0].GetComponent<Weapon>().type)
        {
            ClearAllWeaponSlots();
            weapon = weaponManager.GetWeapon(type);
            currentWeapons[0] = SetWeaponPosition(weapon, weaponSlots[0]);
            currentWeaponType[0] = currentWeapons[0].GetComponent<Weapon>();
            currentWeaponType[0].currentDamage = currentWeaponType[0].defDamage * damageMult;
        }
        else if (type == WeaponType.laserGun) return;
        else 
        {
            if (currentWeapons[1] == null)
            {
                currentWeapons[1] = SetWeaponPosition(weapon, weaponSlots[1]);
                currentWeaponType[1] = currentWeapons[1].GetComponent<Weapon>();
                currentWeaponType[1].currentDamage = currentWeaponType[1].defDamage / 2 * damageMult;
                currentWeaponType[1].lastShootTime = currentWeaponType[0].lastShootTime;
            }
            else if (currentWeapons[2] == null)
            {
                currentWeapons[2] = SetWeaponPosition(weapon, weaponSlots[2]);
                currentWeaponType[2] = currentWeapons[2].GetComponent<Weapon>();
                currentWeaponType[2].currentDamage = currentWeaponType[2].defDamage / 2 * damageMult;
                currentWeaponType[2].lastShootTime = currentWeaponType[0].lastShootTime;

            }
        }
    }


    private void SetSpecWeapon(WeaponType type)
    {
        GameObject weapon = weaponManager.GetWeapon(type);
        if (currentSpecWeapType.type != type)
        {
            ClearSpecSlot();
            currentSpecWeap = SetWeaponPosition(weapon, specSlot);
            currentSpecWeapType = currentSpecWeap.GetComponent<Weapon>();
            currentSpecWeapType.currentDamage = currentSpecWeapType.defDamage;
            UpdateCharges(1);
        }
        else
        {
            if (_specCharges < 3) 
            {
                UpdateCharges(++_specCharges);
            }
        }
    }

    private void ClearSpecSlot()
    {
        Destroy(currentSpecWeap.gameObject);
        currentSpecWeapType = null;
    }

    private void ClearAllWeaponSlots()
    {
        for (int i = 0; i < 3; i++)
        {
            if (currentWeaponType[i] != null)
            {
                Destroy(currentWeapons[i].gameObject);
                currentWeaponType[i] = null;
            }
        }
    }

    private void UpdateCharges(int charges)
    {
        _specCharges = charges;
        OnChargesSpend?.Invoke(_specCharges);
    }

    public void UpdateDamage()
    {
        damageMult += 0.1f;
        for (int i = 0; i < currentWeaponType.Length; i++)
        {
            if (currentWeaponType[i] != null)
            {
                if (i == 0) currentWeaponType[i].currentDamage = currentWeaponType[i].defDamage * damageMult;
                else
                {
                    currentWeaponType[i].currentDamage = currentWeaponType[i].defDamage / 2 * damageMult;
                }
            }
        }
        currentSpecWeapType.currentDamage = currentSpecWeapType.defDamage * damageMult;
        playerUI.UpdateDamage(damageMult);
    }
}


