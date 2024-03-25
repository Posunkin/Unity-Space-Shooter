using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public void RestartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitToMenuButton()
    {
        SceneManager.LoadScene("StartScene");
    }
}
