using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    protected BoundsCheck bndCheck;
    internal float damageToDeal;

    internal void SetProjectile(float damage)
    {
        damageToDeal = damage;
    }
    
    protected virtual void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    protected virtual void Update()
    {   
        if (bndCheck != null && bndCheck.offUp)
        {
            Destroy(gameObject);
        }
    }

    protected abstract void OnCollisionEnter(Collision other);
    protected abstract void OnTriggerEnter();
    
}
