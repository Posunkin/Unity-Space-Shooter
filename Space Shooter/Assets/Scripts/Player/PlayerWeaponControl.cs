using SpaceShooter.Weapons;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerWeaponControl : WeaponControl
{
    [SerializeField] private PlayerStats player;
    [SerializeField] private WeaponType firstWeaponType;
    private GameObject[] currentWeapons = new GameObject[3];
    private Weapon[] playerWeapons = new Weapon[3];

    private void Awake()
    {
        player.OnWeaponAbsorb += SetNewWeapon;
        GameObject weapon = weaponManager.GetWeapon(firstWeaponType);
        currentWeapons[0] = SetWeaponPosition(weapon, 0);
        playerWeapons[0] = currentWeapons[0].GetComponent<Weapon>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpecialWeaponShot?.Invoke();
        }
    }

    private void OnDisable()
    {
        player.OnWeaponAbsorb -= SetNewWeapon;
    }

    private void SetNewWeapon(WeaponType type)
    {
        GameObject weapon = weaponManager.GetWeapon(type);
        if (type != currentWeapons[0].GetComponent<Weapon>().type)
        {
            ClearAllWeaponSlots();
            weapon = weaponManager.GetWeapon(type);
            currentWeapons[0] = SetWeaponPosition(weapon, 0);
            playerWeapons[0] = currentWeapons[0].GetComponent<Weapon>();
            playerWeapons[0].currentDamage = playerWeapons[0].defDamage;
        }
        else
        {
            if (currentWeapons[1] == null)
            {
                currentWeapons[1] = SetWeaponPosition(weapon, 1);
                playerWeapons[1] = currentWeapons[1].GetComponent<Weapon>();
                playerWeapons[1].currentDamage = playerWeapons[1].defDamage / 2;
                playerWeapons[1].lastShootTime = playerWeapons[0].lastShootTime;
            }
            else
            {
                currentWeapons[2] = SetWeaponPosition(weapon, 2);
                playerWeapons[2] = currentWeapons[2].GetComponent<Weapon>();
                playerWeapons[2].currentDamage = playerWeapons[2].defDamage / 2;
                playerWeapons[2].lastShootTime = playerWeapons[0].lastShootTime;

            }
        }
    }

    private void ClearAllWeaponSlots()
    {
        for (int i = 0; i < currentWeapons.Length; i++)
        {
            if (currentWeapons[i] != null) 
            {
                Destroy(currentWeapons[i].gameObject);
                playerWeapons[i] = null;
            }
        }
    }
}
