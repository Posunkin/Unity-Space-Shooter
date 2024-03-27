using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class Language : MonoBehaviour
{
    [DllImport("__Internal")] private static extern string GetLang();

    public string CurrentLanguage = "NO"; // ru en

    public static Language Instance;
    [SerializeField] private TextMeshProUGUI _languageText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        CurrentLanguage = GetLang();
       _languageText.text = CurrentLanguage;
    }
}
