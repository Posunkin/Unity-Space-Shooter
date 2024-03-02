using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class Shield : MonoBehaviour
{
    [SerializeField] private float rotationsPerSecond;
    [SerializeField] private GameObject player;

    [Header("Changing color parameters")]
    [SerializeField] private float changingDuration;
    private float startChangingTime;
    private bool changingColors;

    private int shownLevel = 0;
    private float rotationZ;
    private Material material;
    private Color originalColor;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        originalColor = material.color;
        StartCoroutine(nameof(ChangeColor));
    }

    private void Update()
    {
        if (!changingColors) StartCoroutine(nameof(ChangeColor));
        rotationZ = -(rotationsPerSecond * Time.time * 360) % 360;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    private IEnumerator ChangeColor()
    {
        changingColors = true;
        startChangingTime = Time.time;
        while (Time.time - startChangingTime < changingDuration)
        {
            float t = (Time.time - startChangingTime) / changingDuration;
            Color newColor = Color.Lerp(originalColor, Color.white, t);
            material.color = newColor;
            yield return null;
        }

        startChangingTime = Time.time;
        while (Time.time - startChangingTime < changingDuration)
        {
            float t = (Time.time - startChangingTime) / changingDuration;
            Color newColor = Color.Lerp(Color.white, originalColor, t);
            material.color = newColor;
            yield return null;
        }

        changingColors = false;
        material.color = originalColor;
    }

    internal void ShieldLevelChange(int level)
    {
        if (level <= 4) shownLevel = level;
        else shownLevel = 4;
        material.mainTextureOffset = new Vector2(0.2f * shownLevel, 0);
    } 
}
