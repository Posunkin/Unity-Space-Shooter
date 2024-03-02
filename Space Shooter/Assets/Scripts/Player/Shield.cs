using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [SerializeField] private float rotationsPerSecond;

    [Header("Changing color parameters")]
    [SerializeField] private float changingDuration;
    private float startChangingTime;
    private bool changingColors;

    private int _shownLevel = 0;
    private float _rotationZ;
    private Material _material;
    private Color originalColor;

    private void Awake()
    {
        _material = GetComponent<Renderer>().material;
        originalColor = _material.color;
        StartCoroutine(nameof(ChangeColor));
    }

    private void Update()
    {
        if (!changingColors) StartCoroutine(nameof(ChangeColor));
        _rotationZ = -(rotationsPerSecond * Time.time * 360) % 360;
        transform.rotation = Quaternion.Euler(0, 0, _rotationZ);
    }

    private IEnumerator ChangeColor()
    {
        changingColors = true;
        startChangingTime = Time.time;
        while (Time.time - startChangingTime < changingDuration)
        {
            float t = (Time.time - startChangingTime) / changingDuration;
            Color newColor = Color.Lerp(originalColor, Color.white, t);
            _material.color = newColor;
            yield return null;
        }

        startChangingTime = Time.time;
        while (Time.time - startChangingTime < changingDuration)
        {
            float t = (Time.time - startChangingTime) / changingDuration;
            Color newColor = Color.Lerp(Color.white, originalColor, t);
            _material.color = newColor;
            yield return null;
        }

        changingColors = false;
        _material.color = originalColor;
    }

    internal void ShieldLevelChange(int level)
    {
        if (level <= 4) _shownLevel = level;
        else _shownLevel = 4;
        _material.mainTextureOffset = new Vector2(0.2f * _shownLevel, 0);
    } 
}
