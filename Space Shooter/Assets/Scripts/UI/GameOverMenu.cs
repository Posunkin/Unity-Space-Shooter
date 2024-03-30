using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void RestartButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("GameScene");
    }

    public void ExitToMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");
    }
}
