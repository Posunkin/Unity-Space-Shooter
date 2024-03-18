using TMPro;
using UnityEngine;
using UnityEngine.Events;
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

    public void ExitButton()
    {
        Application.Quit();
    }
}
