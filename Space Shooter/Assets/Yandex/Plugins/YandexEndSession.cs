using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class YandexEndSession : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void RateGame();

    [SerializeField] private TextMeshProUGUI thaksText;
    [SerializeField] private Button rateButton;

    public void Rate()
    {
        RateGame();
    }

    public void RateSucces()
    {
        rateButton.gameObject.SetActive(false);
        thaksText.gameObject.SetActive(true);
    }
}
