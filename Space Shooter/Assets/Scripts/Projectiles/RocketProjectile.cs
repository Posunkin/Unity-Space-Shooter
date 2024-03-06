using System;
using System.Collections;
using SpaceShooter.Enemies;
using Unity.VisualScripting;
using UnityEngine;

public class RocketProjectile : Projectile
{
    // Parameters
    private Rigidbody rb;
    private Transform target;
    private float speed;
    private float explosionRadius = 5;
    private bool isSearching = true;

    // Delay before rocket start moving and acceleration
    private float delayDescentSpeed = 5;
    private float delayTime = 1f;
    private float accelerationRate = 2f;
    private bool isDelayed = true;
    private float currentSpeed = 0f;

    [SerializeField] private GameObject fireEffect;

    protected override void Awake()
    {
        fireEffect.SetActive(false);
        rb = GetComponent<Rigidbody>();
        base.Awake();
    }

    internal void SetProjectile(float damage, float speed)
    {
        damageToDeal = damage;
        this.speed = speed;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        fireEffect.transform.rotation = Quaternion.Euler(-90, 0, 0);
        FindNearestTarget();
        fireEffect.SetActive(true);
    }

    protected override void Update()
    {
        if (isSearching) return;
        else if (isDelayed)
        {
            rb.velocity = -transform.forward * delayDescentSpeed;
            StartCoroutine(nameof(Delay));
        }
        else
        {
            if (target != null)
            {
                Vector3 direction = (target.position - transform.position).normalized;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = rotation;
                fireEffect.transform.rotation = rotation;
                currentSpeed = Mathf.Lerp(currentSpeed, speed, accelerationRate * Time.deltaTime);
                rb.velocity = direction * currentSpeed;
                
            }
            else
            {
                Vector3 direction = Vector3.up;
                currentSpeed = Mathf.Lerp(currentSpeed, speed, accelerationRate * Time.deltaTime);
                rb.velocity = direction * currentSpeed;
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
        base.Update();
    }

    private void FindNearestTarget()
    {
        isSearching = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                nearestEnemy = enemy;
            }

            if (nearestEnemy != null)
            {
                target = nearestEnemy.transform;
                isSearching = false;
            }
        }
        isSearching = false;
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delayTime);
        isDelayed = false;
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            
            Enemy enemy = collider.GetComponentInParent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damageToDeal);
            }
        }

        Destroy(gameObject);
    }

    protected override void OnCollisionEnter(Collision other)
    {
        Explode();
    }

    protected override void OnTriggerEnter()
    {
        // throw new System.NotImplementedException();
    }
}
