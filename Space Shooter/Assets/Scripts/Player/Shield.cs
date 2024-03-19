using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

public class Shield : MonoBehaviour
{
    [SerializeField] private float rotationsPerSecond;

    private int shownLevel = 0;
    private float rotationZ;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        rotationZ = -(rotationsPerSecond * Time.time * 360) % 360;
        transform.rotation = Quaternion.Euler(0, 0, rotationZ);
    }

    internal void ShieldLevelChange(int level)
    {
        if (level <= 4) shownLevel = level;
        else shownLevel = 4;
        material.mainTextureOffset = new Vector2(0.2f * shownLevel, 0);
    } 
}
