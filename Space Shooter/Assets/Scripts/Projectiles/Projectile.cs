using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    internal BoundsCheck bndCheck;
    internal protected float damageToDeal;

    internal void SetProjectile(float damage)
    {
        damageToDeal = damage;
    }
    
    internal void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    internal void Update()
    {   
        if (bndCheck != null && bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    internal abstract void OnCollisionEnter();
}
