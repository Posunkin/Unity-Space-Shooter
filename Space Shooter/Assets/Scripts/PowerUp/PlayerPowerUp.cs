using UnityEngine;

public class PlayerPowerUp : PowerUp
{
    [SerializeField] private PowerUpType powerUpType;
    public PowerUpType[] powerUpTypeFrequency;

    public void SetType(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.shield:
                powerUpType = type;
                rend.material.color = Color.blue;
                letter.text = "S";
                break;
            case PowerUpType.invulnerability:
                powerUpType = type;
                rend.material.color = Color.white;
                letter.text = "I";
                break;
        }
    }

    public PowerUpType GetPowerUpType(int index)
    {
        return powerUpTypeFrequency[index];
    }

    public PowerUpType GetPowerUpType()
    {
        return powerUpType;
    }

    private void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
