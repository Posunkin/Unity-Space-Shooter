using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private PlayerStats player;
    private float gameRestartDelay = 2;
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
        Invoke(nameof(Restart), gameRestartDelay);
    }

    private void Restart()
    {
        SceneManager.LoadScene("GameScene");
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
