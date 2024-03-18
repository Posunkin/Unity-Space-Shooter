using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private PlayerStats player;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private LeaderBoard leaderBoard;

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
