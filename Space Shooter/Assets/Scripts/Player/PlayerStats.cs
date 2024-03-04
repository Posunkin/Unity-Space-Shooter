using System;
using SpaceShooter.Enemies;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour, IDamageable
{
    public Action OnPlayerDeath;

    [Header("Shield status")]
    [SerializeField] private Shield shield;
    [SerializeField] private int shieldLevel;
    private GameObject lastTrigger = null;
    private float lastTriggerDelay = 1;
    private float lastTriggerEnter;

    private void Start()
    {
        shield.ShieldLevelChange(shieldLevel);
    }

    private void TakeDamage()
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

        if (lastTrigger == go) return;

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
        else
        {
            Debug.Log("Triggered with no enemy: " + go.name);
        }
    }
}
