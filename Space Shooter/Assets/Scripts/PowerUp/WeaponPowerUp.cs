using SpaceShooter.Weapons;
using UnityEngine;

public class WeaponPowerUp : PowerUp
{
    private WeaponManager weaponManager;
    [SerializeField] private WeaponType powerWeaponType;

    protected override void Awake()
    {
        base.Awake();
        weaponManager = WeaponManager.S;
        SetType(powerWeaponType);
    }

    public override void SetType(WeaponType weaponType)
    {
        Weapon weap = weaponManager.GetWeapon(weaponType).GetComponent<Weapon>();
        rend.material.color = weap.color;
        letter.text = weap.letter;
        this.powerWeaponType = weaponType;
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
