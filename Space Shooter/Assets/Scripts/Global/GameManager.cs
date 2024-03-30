using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void SetToLeaderboard(int value);
    [DllImport("__Internal")] private static extern void ShowAdd();
    [DllImport("__Internal")] private static extern void ShowRewardedVideo();

    public static GameManager Instance;
    [SerializeField] private PlayerStats player;

    [SerializeField] private GameObject playerUI;
    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private GameObject lastChanceMenu;
    private bool gamePaused = false;
    private float pauseDelay = 0.5f;
    private float lastPause;
    private bool playerHasLastChance = true;

    private List<IScoreObserver> ScoreObservers = new();

    // Music
    private AudioSource audioSource => GetComponent<AudioSource>();
    [SerializeField] private AudioClip backMusic;
    [SerializeField] private AudioClip bossMusic;

    private void OnEnable()
    {
        player.OnPlayerDeath += LastChanceMenu;
    }

    private void OnDisable()
    {
        player.OnPlayerDeath -= LastChanceMenu;
    }

    private void LastChanceMenu(GameObject player)
    {
        if (playerHasLastChance)
        {
            Time.timeScale = 0;
            ShowRewardedButton();
            playerHasLastChance = false;
            return;
        }
        GameOverMenu();
        Destroy(player);
    }

    private void ShowRewardedButton()
    {
        lastChanceMenu.gameObject.SetActive(true);
    }

    public void RewardButton()
    {
        ShowRewardedVideo();
    }

    public void DeniedLastChance()
    {
        lastChanceMenu.gameObject.SetActive(false);
        Time.timeScale = 1;
        Destroy(player.gameObject);
        GameOverMenu();
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

    private void Start()
    {
        BackMusic();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape) && Time.time - lastPause > pauseDelay)
        {
            lastPause = Time.time;
            PauseGame();
        }
    }

    public void PauseGame()
    {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused ? 0 : 1;
        pauseMenu.SetActive(gamePaused);
        playerUI.SetActive(!gamePaused);
    }

    private void GameOverMenu()
    {
        playerUI.SetActive(false);
        gameOverMenu.SetActive(true);
        finalScore.text = playerUI.GetComponent<PlayerUI>().PlayerScore.ToString();
        SetToLeaderboard(int.Parse(finalScore.text));
        // ShowAdd();
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
        foreach (IScoreObserver observer in ScoreObservers)
        {
            observer.UpdateScore(score);
        }
    }

    public void Reward()
    {
        player.LastChance();
        lastChanceMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void BackMusic()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(backMusic);
    }

    public void BossMusic()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(bossMusic);
    }
}
