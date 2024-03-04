using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected BoundsCheck bndCheck;
    internal float damageToDeal;

    internal void SetProjectile(float damage)
    {
        damageToDeal = damage;
    }
    
    protected void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    protected void Update()
    {   
        if (bndCheck != null && bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void OnCollisionEnter();
    protected abstract void OnTriggerEnter();
    
}
