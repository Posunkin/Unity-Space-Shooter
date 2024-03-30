using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour, IScoreObserver
{
    public Action<string, int> SubmitScoreEvent;

    [SerializeField] private TMP_InputField inputName;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI lifeCountText;
    [SerializeField] private TextMeshProUGUI specChargesText;
    [SerializeField] private TextMeshProUGUI endScoreText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private GameObject playerObj;
    private PlayerStats player;
    private PlayerWeaponControl playerWeaponControl;
    private int playerScore = 0;
    public int PlayerScore {get => playerScore;}

    [Header("Weapons UI")]
    [SerializeField] private GameObject rocketWeapon;
    [SerializeField] private GameObject mineWeapon;
    [SerializeField] private GameObject[] weapons; // 0 - blaster, 1 - blaster shotgun, 2 - plasma, 3 - laser


    private void Awake()
    {
        scoreText.text = playerScore.ToString();
        player = playerObj.GetComponent<PlayerStats>();
        playerWeaponControl = playerObj.GetComponent<PlayerWeaponControl>();
        player.OnShieldLevelChange += UpdateLifeCount;
        playerWeaponControl.OnChargesSpend += UpdateSpecCharges;
    }

    private void OnDisable()
    {
        player.OnShieldLevelChange -= UpdateLifeCount;
        playerWeaponControl.OnChargesSpend -= UpdateSpecCharges;

    }

    private void Start()
    {
        GameManager.Instance.AddScoreObserver(this);
    }

    public void UpdateScore(int score)
    {
        playerScore += score;
        if (playerScore < 0) playerScore = 0;
        scoreText.text = playerScore.ToString();
    }

    private void UpdateLifeCount(int count)
    {
        switch (count)
        {
            case 0:
                lifeCountText.text = " ";
                break;
            case 1:
                lifeCountText.text = "I";
                break;
            case 2:
                lifeCountText.text = "II";
                break;
            case 3:
                lifeCountText.text = "III";
                break;
            case 4:
                lifeCountText.text = "IIII";
                break;
        }
    }

    private void UpdateSpecCharges(int charges)
    {
        switch(charges)
        {
            case 0:
                specChargesText.text = " ";
                break;
            case 1:
                specChargesText.text = "I";
                break;
            case 2:
                specChargesText.text = "II";
                break;
            case 3:
                specChargesText.text = "III";
                break;
        }
    }

    public void UpdateDamage(float damageMult)
    {
        damageText.text = damageMult.ToString();
    }

    public void NewSpecWeapon(WeaponType type)
    {
        switch (type)
        {
            case WeaponType.rocketLauncher:
                rocketWeapon.SetActive(true);
                mineWeapon.SetActive(false);
                break;
            case WeaponType.mineTrap:
                rocketWeapon.SetActive(false);
                mineWeapon.SetActive(true);
                break;
        }
    }

    public void NewWeapon(WeaponType type)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            if (i == (int) type) weapons[i].gameObject.SetActive(true);
            else weapons[i].gameObject.SetActive(false);
        }
    }
}
