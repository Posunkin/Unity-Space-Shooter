using System;
using System.Collections;
using SpaceShooter.Enemies;
using Unity.VisualScripting;
using UnityEngine;

public class RocketProjectile : Projectile
{
    [SerializeField] private GameObject rocketExplosion;
    [SerializeField] private GameObject rocketSmoke;
    // Parameters
    private Rigidbody rb;
    private Transform target;
    private float speed;
    private float explosionRadius = 5;
    private bool isSearching = true;
    private Vector3 direction;

    // Delay before rocket start moving and acceleration
    private float delayDescentSpeed = 5;
    private float delayTime = 1f;
    private float accelerationRate = 2f;
    private bool isDelayed = true;
    private float currentSpeed = 0f;

    protected override void Awake()
    {
        rb = GetComponent<Rigidbody>();
        base.Awake();
    }

    internal void SetProjectile(float damage, float speed)
    {
        damageToDeal = damage;
        this.speed = speed;
        transform.rotation = Quaternion.Euler(-90, 0, 0);
        FindNearestTarget();
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
            rocketSmoke.SetActive(true);
            if (target != null)
            {
                direction = (target.position - transform.position).normalized;
                transform.rotation = Quaternion.LookRotation(direction);
                MoveToDirection();
                
            }
            else
            {
                direction = Vector3.up;
                if (currentSpeed <= 0) currentSpeed = 0;
                rb.MoveRotation(Quaternion.Slerp(transform.rotation, Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, 0), currentSpeed * Time.deltaTime));
                currentSpeed = Mathf.Lerp(currentSpeed, speed, accelerationRate * Time.deltaTime);
                rb.velocity = transform.forward * currentSpeed;
            }
        }
        base.Update();
    }

    private void MoveToDirection()
    {
        currentSpeed = Mathf.Lerp(currentSpeed, speed, accelerationRate * Time.deltaTime);
        rb.velocity = direction * currentSpeed;
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

    private void Explode(Transform enemyPos)
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
        GameObject go = Instantiate(rocketExplosion);
        go.transform.position = enemyPos.position;
        Destroy(gameObject);
    }

    protected void OnCollisionEnter(Collision other)
    {
        Explode(other.transform);
    }
}
