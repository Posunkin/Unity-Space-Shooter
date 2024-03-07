using System;
using SpaceShooter.Enemies;
using UnityEngine;

/// <summary>
/// Includes hp and triggers for player
/// </summary>

public class PlayerStats : MonoBehaviour, IDamageable
{
    public Action OnPlayerDeath;
    public Action<WeaponType> OnWeaponAbsorb;

    [Header("Shield status")]
    [SerializeField] private Shield shield;
    [SerializeField] private int shieldLevel;
    private GameObject lastTrigger = null;
    private float lastTriggerDelay = 1;
    private float lastTriggerEnter;

    private PlayerWeaponControl weaponControl;

    private void Start()
    {
        shield.ShieldLevelChange(shieldLevel);
        weaponControl = GetComponent<PlayerWeaponControl>();
    }

    public void TakeDamage()
    {
        shieldLevel--;
        if (shieldLevel < 0)
        {
            OnPlayerDeath?.Invoke();
            Destroy(gameObject);
        }
        else shield.ShieldLevelChange(shieldLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (lastTriggerEnter < Time.time + lastTriggerDelay) lastTrigger = null;
        Transform root = other.gameObject.transform.root;
        GameObject go = root.gameObject;

        if (lastTrigger == go && go.GetComponent<PowerUp>() == null) return;

        lastTrigger = go;
        lastTriggerEnter = Time.time;
        if (other.gameObject.GetComponent<Projectile>() != null)
        {
            TakeDamage();
        }
        else if (go.GetComponent<Enemy>() != null)
        {
            TakeDamage();
        }
        else if (go.GetComponent<PowerUp>() != null)
        {
            if (go.GetComponent<WeaponPowerUp>() != null)
            {
                WeaponPowerUp pwr = go.GetComponent<WeaponPowerUp>();
                OnWeaponAbsorb?.Invoke(pwr.GetWeaponType());
            }
        }
        else
        {
            Debug.Log("Triggered with no enemy: " + go.name);
        }
    }
}
