using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.Networking;

public class YandexSDK : MonoBehaviour
{
    [DllImport("__Internal")] private static extern void SetPlayerData();

    [SerializeField] private TextMeshProUGUI _nameText;
    [SerializeField] private RawImage _photo;

    public void Authorize()
    {
        _nameText.gameObject.SetActive(true);
        _photo.gameObject.SetActive(true);

        SetPlayerData(); 
    }

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public void SetPhoto(string url)
    {
        StartCoroutine(DownloadImage(url));
    }

    private IEnumerator DownloadImage(string mediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(mediaUrl);
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            _photo.texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
        }
    }
}
