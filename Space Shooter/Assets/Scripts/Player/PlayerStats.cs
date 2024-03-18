using System;
using System.Collections;
using SpaceShooter.Enemies;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public Action OnPlayerDeath;
    public Action<WeaponType> OnWeaponAbsorb;

    [Header("Shield status")]
    public Action<int> OnShieldLevelChange;
    [SerializeField] private Shield shield;
    [SerializeField] private int shieldLevel;

    [Header("Invulnerability stats:")]
    [SerializeField] private float invulnerabilityTime;
    [SerializeField] private float invulAfterTakeDamage;
    public bool isInvulnerable = false;

    private PlayerWeaponControl weaponControl;
    private Animator anim;

    private void Start()
    {
        shield.ShieldLevelChange(shieldLevel);
        OnShieldLevelChange?.Invoke(shieldLevel);
        weaponControl = GetComponent<PlayerWeaponControl>();
        anim = GetComponent<Animator>();
    }

    public void TakeDamage()
    {
        if (isInvulnerable) return;
        shieldLevel--;
        GameManager.Instance.UpdateScore(-10);
        StartCoroutine(Invulnerability(invulAfterTakeDamage));
        if (shieldLevel < 0)
        {
            OnPlayerDeath?.Invoke();
            Destroy(gameObject);
        }
        else
        {
            shield.ShieldLevelChange(shieldLevel);
            OnShieldLevelChange?.Invoke(shieldLevel);

        }
    }

    private IEnumerator Invulnerability(float duration)
    {
        isInvulnerable = true;
        anim.SetBool("Invulerable", true);
        yield return new WaitForSeconds(duration);
        anim.SetBool("Invulerable", false);
        isInvulnerable = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform root = other.gameObject.transform.root;
        GameObject go = root.gameObject;

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
            GameManager.Instance.UpdateScore(10);

            if (go.GetComponent<WeaponPowerUp>() != null)
            {
                WeaponPowerUp pwr = go.GetComponent<WeaponPowerUp>();
                OnWeaponAbsorb?.Invoke(pwr.GetWeaponType());
            }
            else 
            {
                PlayerPowerUp pwr = go.GetComponent<PlayerPowerUp>();
                PowerUpType type = pwr.GetPowerUpType();
                switch (type)
                {
                    case PowerUpType.shield:
                        if (shieldLevel < 4)
                        {
                            shieldLevel++;
                            shield.ShieldLevelChange(shieldLevel);
                            OnShieldLevelChange?.Invoke(shieldLevel);
                        }
                        break;
                    case PowerUpType.invulnerability:
                        StartCoroutine(Invulnerability(invulnerabilityTime));
                        break;
                }
            }
        }
        else
        {
            Debug.Log("Triggered with no enemy: " + go.name);
        }
    }
}
