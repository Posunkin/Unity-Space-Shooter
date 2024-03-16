using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;

    private Renderer rend;
    private float offset;

    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        offset += Time.deltaTime * speed;
        rend.material.mainTextureOffset = new Vector2(0, offset);
    }
}
