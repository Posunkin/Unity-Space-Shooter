using System.Collections.Generic;
using SpaceShooter.Weapons;
using UnityEngine;

public enum WeaponType
{
    blaster = 0,
    blasterShotgun = 1
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
                default:
                    return null;
            }
        }
        return null;
    }
}
