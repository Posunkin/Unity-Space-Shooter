using SpaceShooter.Enemies;
using UnityEngine;

public class PlasmaProjectile : Projectile
{
    private Rigidbody rb;
    private int maxBounces = 3;
    private int bouncesCount = 0;
    private float birthTime;
    private float lifeTime = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        birthTime = Time.time;
    }

    protected override void Update()
    {
        if (Time.time >= birthTime + lifeTime)
        {
            Destroy(gameObject);
        }
    }

    protected void OnCollisionEnter(Collision other)
    {
        IDamageable damageable = other.gameObject.GetComponent<IDamageable>();
        if (damageable != null) damageable.TakeDamage(damageToDeal);
        bouncesCount++;
        damageToDeal += 0.5f;
        if (bouncesCount >= maxBounces)
        {
            Destroy(gameObject);
        }
        
    }

    protected void OnTriggerEnter()
    {
        Destroy(gameObject);
    }
}
