using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck bndCheck;
    
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    private void Update()
    {   
        if (bndCheck != null && bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }
}
