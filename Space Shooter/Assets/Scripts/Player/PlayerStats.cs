using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Shield status")]
    [SerializeField] private Shield shield;
    [SerializeField] private int _shieldLevel;

    private void Start()
    {
        shield.ShieldLevelChange(_shieldLevel);
    }
}
