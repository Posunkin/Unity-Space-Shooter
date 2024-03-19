using UnityEngine;

public class SundiverRocket : Projectile
{
    [SerializeField] private GameObject explosion;
    [SerializeField] private GameObject rocketSmoke;
    private Rigidbody rb;
    private Transform target;
    private float speed;
    private float explosionRadius;
    private float delayDescentSpeed = 5f;
    private float delayTime = 1f;
    private float delayStart;
    private bool isDelayed = true;
    private Vector3 direction;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        base.Awake();
    }

    public void SetProjectile(float explosionRadius, float speed, bool isPlayer, Transform target)
    {
        this.explosionRadius = explosionRadius;
        this.speed = speed;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        this.target = target;
        delayStart = Time.time;
    }

    protected override void Update()
    {
        if (isDelayed)
        {
            rb.velocity = -transform.forward * delayDescentSpeed;
            if (Time.time - delayStart < delayTime) isDelayed = false;
        }
        rocketSmoke.SetActive(true);
        direction = (target.position - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
        MoveToDirection();
    }

    private void MoveToDirection()
    {
        rb.velocity = direction * speed;
        if (Vector3.Distance(transform.position, target.position) <= 0.7f) 
        {
            Explode();
        }
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            PlayerStats player = collider.GetComponentInParent<PlayerStats>();
            if (player != null) player.TakeDamage();
        }

        GameObject go = Instantiate(explosion);
        go.transform.position = target.position;
        Destroy(target.gameObject);
        Destroy(gameObject);
    }

    protected void OnCollisionEnter(Collision other)
    {
        Explode();
    }

    protected void OnTriggerEnter(Collider other)
    {
        Explode();
    }
}        
    