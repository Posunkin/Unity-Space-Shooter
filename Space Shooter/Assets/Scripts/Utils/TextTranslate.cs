using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTranslate : MonoBehaviour
{
    [SerializeField] string _en;
    [SerializeField] string _ru;

    private void Start()
    {
        TextMeshProUGUI Text = GetComponent<TextMeshProUGUI>();
        switch (Language.Instance.CurrentLanguage)
        {
            case "en":
                Text.text = _en;
                break;
            case "ru":
                Text.text = _ru;
                break;
            default:
                Text.text = _en;
                break;
        }
    }
}
