using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Enemy_0 : Enemy
    {
        private void Awake()
        {
            bndCheck = GetComponent<BoundsCheck>();
        }

        private void Update()
        {
            base.Move();
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

        protected override void CheckBounds()
        {
            if (bndCheck != null && bndCheck.offDown)
            {
                Destroy(gameObject);
            }
        }

        protected override void OnTriggerEnter(Collider other)
        {
            Transform root = other.gameObject.transform.root;
            GameObject go = root.gameObject;
                    
            if (go.GetComponent<PlayerStats>() != null)
            {
                TakeDamage(damageOnTrigger);
            }
            else
            {
                Debug.Log("Triggered with " + other.gameObject.name);
            }
        }

        protected override void OnCollisionEnter(Collision other)
        {
            Projectile proj = other.gameObject.GetComponent<Projectile>();
            if (proj != null)
            {
                TakeDamage(proj.damageToDeal);
            }
            else
            {
                Debug.Log("Collision with " + other.gameObject.name);
            }
        }
    }
}
