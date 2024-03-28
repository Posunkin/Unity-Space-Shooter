using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void SetToLeaderboard(int value);
    [DllImport("__Internal")] private static extern void ShowAdd();

    public static GameManager Instance;
    [SerializeField] private PlayerStats player;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private TextMeshProUGUI finalScore;

    private List<IScoreObserver> ScoreObservers = new();

    private void OnEnable()
    {
        player.OnPlayerDeath += GameOverMenu;
    }

    private void OnDisable()
    {
        player.OnPlayerDeath -= GameOverMenu;
    }

    private void Awake()
    {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void GameOverMenu()
    {
        playerUI.SetActive(false);
        gameOverMenu.SetActive(true);
        finalScore.text = playerUI.GetComponent<PlayerUI>().PlayerScore.ToString();
        SetToLeaderboard(int.Parse(finalScore.text));
        ShowAdd();
    }

    public void AddScoreObserver(IScoreObserver observer)
    {
        ScoreObservers.Add(observer);
    }

    public void RemoveScoreObserver(IScoreObserver observer)
    {
        ScoreObservers.Remove(observer);
    }

    public void UpdateScore(int score)
    {
        foreach(IScoreObserver observer in ScoreObservers)
        {
            observer.UpdateScore(score);
        }
    }
}
