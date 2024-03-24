using Unity.VisualScripting;
using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Space_Keeper : Enemy
    {
        [Header("Space Keeper parameters:")]
        [SerializeField] private BossHealthBar healthBar;
        [SerializeField] private EnemyWeaponControl basicWeapons;
        [SerializeField] private EnemyWeaponControl rocketWeapon;
        [SerializeField] private GameObject avoidEffect;
        [SerializeField] private float moveTime;

        // Movement
        private float sinEccentricity = 0.6f;
        private Vector3 p0;
        private Vector3 p1;
        private Vector2 yCoordinates;
        private Vector3[] points;
        private float startMoveTime;
        private bool firstMovementState = true;
        private bool haveCoordinates = false;

        private Spawner spawner;
        private bool spawingTeam = false;

        private void Start()
        {
            spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            yCoordinates = new Vector2(-bndCheck.CamHeight, bndCheck.CamHeight);
            basicWeapons.Stop();
            healthBar.SetMaxHealth(maxHealth);
        }

        private void Update()
        {
            if (firstMovementState) 
            {
                if (!haveCoordinates) SetSideCoordinates();
                MoveSide();
            }
            else
            {
                if (!haveCoordinates) SetCoordinates();
                Move();
            }
        }

        private void SetSideCoordinates()
        {
            rocketWeapon.Activate();
            p0 = Vector3.zero;
            p0.x = -bndCheck.CamWidth - bndCheck.Radius;
            p0.y = Random.Range(yCoordinates.x, yCoordinates.y);

            p1 = Vector3.zero;
            p1.x = bndCheck.CamWidth + bndCheck.Radius;
            p1.y = Random.Range(yCoordinates.x, yCoordinates.y);

            if (Random.value > 0.5f)
            {
                p0.x *= -1;
                p1.x *= -1;
            }

            haveCoordinates = true;
            startMoveTime = Time.time;
        }

        private void MoveSide()
        {
            float u = (Time.time - startMoveTime) / moveTime;

            if (u > 1)
            {
                firstMovementState = false;
                haveCoordinates = false;
                if (currentHealth > maxHealth * 0.5f) rocketWeapon.Stop();
            }

            u = u + sinEccentricity * (Mathf.Sin(u * Mathf.PI * 2));

            pos = (1 - u) * p0 + u * p1;
        }

        private void SetCoordinates()
        {
            basicWeapons.Activate();
            points = new Vector3[3];
            points[0] = pos;

            float xMin = -bndCheck.CamWidth + bndCheck.Radius;
            float xMax = bndCheck.CamWidth - bndCheck.Radius;

            Vector3 v = Vector3.zero;
            v.x = Random.Range(xMin, xMax);
            v.y = -bndCheck.CamHeight * Random.Range(2.75f, 2);
            points[1] = v;

            v = Vector3.zero;
            v.y = pos.y;
            v.x = Random.Range(xMin, xMax);
            points[2] = v;

            if (transform.position.y < 0)
            {
                points[1] *= -1;
                points[2] *= -1;
            }

            startMoveTime = Time.time;
            haveCoordinates = true;
        }

        protected override void Move()
        {
            float u = (Time.time - startMoveTime) / moveTime;

            if (u > 1)
            {
                haveCoordinates = false;
                firstMovementState = true;
                if (currentHealth > maxHealth * 0.5f) basicWeapons.Stop();
                GameObject go = Instantiate(avoidEffect);
                go.transform.position = transform.position;
            }

            Vector3 p01, p12;
            p01 = (1 - u) * points[0] + u * points[1];
            p12 = (1 - u) * points[1] + u * points[2];
           
            pos = (1 - u) * p01 + u * p12;
        }
    
        public override void TakeDamage()
        {
            base.TakeDamage();
            healthBar.ChangeHealth(currentHealth);
        }
        
        public override void TakeDamage(float damage)
        {
            base.TakeDamage(damage);
            if (currentHealth < maxHealth * 0.25f && !spawingTeam)
            {   
                InvokeRepeating(nameof(SpawnBossTeam), 2, 2);
                spawingTeam = true;
            }   
            healthBar.ChangeHealth(currentHealth);
        }

        private void SpawnBossTeam()
        {
            spawner.SpawnBossTeam("Space_Keeper");
        }
    }
}

