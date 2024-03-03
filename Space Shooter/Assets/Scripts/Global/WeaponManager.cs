using System.Collections.Generic;
using SpaceShooter.Weapons;
using UnityEngine;

public enum WeaponType
{
    blaster
}

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] weapons;
    public static Dictionary<WeaponType, GameObject> WEAP_DICT;

    private void Awake()
    {
        WEAP_DICT = new Dictionary<WeaponType, GameObject>();
        foreach (GameObject weapon in weapons)
        {
            WEAP_DICT[weapon.GetComponent<Weapon>().type] = weapon;
        }
    }

    public GameObject SetWeapon(WeaponType type)
    {
        if (WEAP_DICT.ContainsKey(type))
        {
            switch (type)
            {
                case WeaponType.blaster:
                    return WEAP_DICT[WeaponType.blaster];
                default:
                    return null;
            }
        }
        return null;
    }
}
