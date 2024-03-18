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
    [SerializeField] private RawImage[] specWeaponImages; // 0 = rocket, 1 = mine
    [SerializeField] private GameObject playerObj;
    private PlayerStats player;
    private PlayerWeaponControl playerWeaponControl;
    private int startScore = 0;

    private void Awake()
    {
        scoreText.text = startScore.ToString();
        player = playerObj.GetComponent<PlayerStats>();
        playerWeaponControl = playerObj.GetComponent<PlayerWeaponControl>();
        player.OnPlayerDeath += LeaderboardUpdate;
        player.OnShieldLevelChange += UpdateLifeCount;
        playerWeaponControl.OnChargesSpend += UpdateSpecCharges;
    }

    private void OnDisable()
    {
        player.OnPlayerDeath -= LeaderboardUpdate;
        player.OnShieldLevelChange -= UpdateLifeCount;
        playerWeaponControl.OnChargesSpend -= UpdateSpecCharges;

    }

    private void Start()
    {
        GameManager.Instance.AddScoreObserver(this);
    }

    public void UpdateScore(int score)
    {
        startScore += score;
        if (startScore < 0) startScore = 0;
        scoreText.text = startScore.ToString();
    }

    public void UpdateSpecWeapon(WeaponType type)
    {
        switch(type)
        {
            case WeaponType.rocketLauncher:
                specWeaponImages[0].gameObject.SetActive(true);
                specWeaponImages[1].gameObject.SetActive(false);
                break;
            case WeaponType.mineTrap:
                specWeaponImages[1].gameObject.SetActive(true);
                specWeaponImages[0].gameObject.SetActive(false);
                break;
        }
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

    public void LeaderboardUpdate()
    {
        endScoreText.text = startScore.ToString();
        SubmitScoreEvent?.Invoke(inputName.text, int.Parse(scoreText.text));
    }
}
