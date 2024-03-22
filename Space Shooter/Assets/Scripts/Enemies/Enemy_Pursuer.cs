using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Enemy_Pursuer : Enemy
    {
        [SerializeField] private float lifeTime;
        [SerializeField] private float explosionRadius;
        [SerializeField] private GameObject explosion;
        private Transform target;
        Vector3 direction;
        private float birthTime;
        
        private void Start()
        {
            target = GameObject.Find("Player")?.transform;
            birthTime = Time.time;
        }

        private void Update()
        {
            if (Time.time >= lifeTime + birthTime)
            {
                Explode();
            }

            if (target != null)
            {
                Move();
            }
        }

        protected override void Move()
        {
            direction = (target.position - transform.position).normalized;
            Quaternion rotate = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Euler(rotate.eulerAngles.x, rotate.eulerAngles.y, -90);
            transform.position += direction * speed * Time.deltaTime;
        }

        public override void TakeDamage(float damage)
        {
            currentHealth -= damage;
            StartCoroutine(nameof(ShowDamage));
            if (currentHealth <= 0)
            {
                GameManager.Instance.UpdateScore(score);
                Explode();
            }
            if (currentHealth <= maxHealth / 2)
            {
                smokeEffect.Play();
            }
        }

        private void Explode()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

            foreach (Collider collider in colliders)
            {
                PlayerStats playerStats = collider.GetComponentInParent<PlayerStats>();
                if (playerStats != null)
                {
                    playerStats.TakeDamage();
                }
            }
            GameObject go = Instantiate(explosion);
            go.transform.position = transform.position;
            Death(true);
        }
    }
}

