using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public Action OnPlayerDeath;

    [Header("Shield status")]
    [SerializeField] private Shield shield;
    [SerializeField] private int shieldLevel;
    private GameObject lastTrigger = null;

    private void Start()
    {
        shield.ShieldLevelChange(shieldLevel);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform root = other.gameObject.transform.root;
        GameObject go = root.gameObject;
        
        // Check for last trigger
        if (lastTrigger == go) return;

        lastTrigger = go;
        if (go.tag == "Enemy")
        {
            shieldLevel--;
            if (shieldLevel < 0) 
            {
                OnPlayerDeath?.Invoke();
                Destroy(gameObject);
            }
            else shield.ShieldLevelChange(shieldLevel);
        }
        else
        {
            Debug.Log("Triggered with no enemy: " + go.name);
        }
    }
}
