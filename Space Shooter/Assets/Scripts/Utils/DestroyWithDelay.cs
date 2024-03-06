using UnityEngine;

public class DestroyWithDelay : MonoBehaviour
{
    [SerializeField] private float delayTime;

    private void Awake()
    {
        Destroy(gameObject, delayTime);
    }
}
