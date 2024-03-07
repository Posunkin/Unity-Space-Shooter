using System.Collections.Generic;
using SpaceShooter.Weapons;
using UnityEngine;

public enum WeaponType
{
    blaster,
    blasterShotgun,
    rocketLauncher,
    mineTrap
}

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager S;
    [SerializeField] private GameObject[] weapons;
    public static Dictionary<WeaponType, GameObject> WEAP_DICT;

    private void Awake()
    {
        S = this;
        WEAP_DICT = new Dictionary<WeaponType, GameObject>();
        foreach (GameObject weapon in weapons)
        {
            WEAP_DICT[weapon.GetComponent<Weapon>().type] = weapon;
        }
    }

    public GameObject GetWeapon(WeaponType type)
    {
        if (WEAP_DICT.ContainsKey(type))
        {
            switch (type)
            {
                case WeaponType.blaster:
                    return WEAP_DICT[WeaponType.blaster];
                case WeaponType.blasterShotgun:
                    return WEAP_DICT[WeaponType.blasterShotgun];
                case WeaponType.rocketLauncher:
                    return WEAP_DICT[WeaponType.rocketLauncher];
                case WeaponType.mineTrap:
                    return WEAP_DICT[WeaponType.mineTrap];
                default:
                    return null;
            }
        }
        return null;
    }
}
