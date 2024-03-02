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

    internal override void Move()
    {
        pos = transform.position;
        pos.y -= speed * Time.deltaTime;
        transform.position = pos;
    }

    internal override void CheckBounds()
    {
        if (bndCheck != null && !bndCheck.offDown)
        {
            // Destroy(gameObject);
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
}