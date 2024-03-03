using UnityEngine;
using SpaceShooter.Enemies;

public class Enemy_0 : Enemy
{
    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    private void Update()
    {
        Move();
        CheckBounds();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    internal override void Move()
    {
        pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        transform.position = pos;
    }

    internal override void CheckBounds()
    {
        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
            Debug.Log("I'm out off bounds!");   
        }
    }

    internal override void OnTriggerEnter(Collider other)
    {
        Transform root = other.gameObject.transform.root;
        GameObject go = root.gameObject;

        if (go.tag == "Player")
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Triggered with no player: " + go.name);
        }
    }

    internal override void OnCollisionEnter(Collision other)
    {
        Projectile proj = other.gameObject.GetComponent<Projectile>();
        if (proj != null)
        {
            TakeDamage(proj.damageToDeal);
            Debug.Log("Player shoot me!");
        }
    }
}