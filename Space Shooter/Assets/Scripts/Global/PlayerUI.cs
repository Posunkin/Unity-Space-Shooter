using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour, IScoreObserver, ILifeObserver
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private RawImage[] lifeCounter;
    private int startScore = 0;
    private int lifeCount;

    private void Awake()
    {
        scoreText.text = startScore.ToString();
    }

    private void Start()
    {
        GameManager.Instance.AddScoreObserver(this);
        GameManager.Instance.AddLifeObserver(this);
    }

    public void UpdateScore(int score)
    {
        startScore += score;
        if (startScore < 0) startScore = 0;
        scoreText.text = startScore.ToString();
    }

    public void UpdateLifeCount(int count)
    {
        for (int i = 0; i < lifeCounter.Length; i++)
        {
            if (i > count - 1) lifeCounter[i].enabled = false;
            else lifeCounter[i].enabled = true;
        }
    }
}
