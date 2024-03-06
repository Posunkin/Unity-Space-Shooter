using SpaceShooter.Weapons;
using UnityEngine;

public class WeaponPowerUp : PowerUp
{
    [SerializeField] private WeaponType powerWeaponType;
    public WeaponType[] weaponTypeFrequency;

    public override void SetType(WeaponType weaponType)
    {
        Weapon weap = WeaponManager.S.GetWeapon(weaponType).GetComponent<Weapon>();
        powerWeaponType = weap.type;
        rend.material.color = weap.color;
        letter.text = weap.letter;
        this.powerWeaponType = weaponType;
    }

    public WeaponType GetWeaponType(int index)
    {
        return weaponTypeFrequency[index];
    }

    public WeaponType GetWeaponType()
    {
        return powerWeaponType;
    }

    private void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
