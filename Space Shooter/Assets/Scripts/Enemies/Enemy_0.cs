using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Enemy_0 : Enemy
    {
        private void Update()
        {
            base.Move();
            CheckBounds();
        }

        protected override void CheckBounds()
        {
            if (bndCheck != null && bndCheck.offDown)
            {
                Death();
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
            {
                Debug.Log("Collision with " + other.gameObject.name);
            }
        }
    }
}
