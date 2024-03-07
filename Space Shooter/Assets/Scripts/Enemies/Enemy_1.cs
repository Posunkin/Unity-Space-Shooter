using System.Collections;
using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Enemy_1 : Enemy
    {
        // Enemy 1 moves along a sinusoid
        [Header("Moving parameters:")]
        [SerializeField] private float waveFrequency;
        [SerializeField] private float waveWidth;
        [SerializeField] private float waveRotY;

        private float x0; // starting coordinate
        private float birthTime;

        private void Start()
        {
            x0 = pos.x;
            birthTime = Time.time;
        }

        private void Update()
        {
            Move();
            CheckBounds();
        }

        protected override void Move()
        {
            tempPos = pos;
            float age = Time.time - birthTime;
            float theta = Mathf.PI * 2 * age / waveFrequency;
            float sin = Mathf.Sin(theta);
            tempPos.x = x0 + waveWidth * sin;
            pos = tempPos;

            // Rotate Y
            Vector3 rot = new Vector3(0, sin * waveRotY, 0);
            this.transform.rotation = Quaternion.Euler(rot);

            base.Move();
        }

        private void CheckBounds()
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
    }
}
