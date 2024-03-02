using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerStats player;
    private float gameRestartDelay = 2;

    private void OnEnable()
    {
        player.OnPlayerDeath += GameOverMenu;
    }

    private void OnDisable()
    {
        player.OnPlayerDeath -= GameOverMenu;
    }

    private void GameOverMenu()
    {
        Invoke(nameof(Restart), gameRestartDelay);
    }

    private void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
