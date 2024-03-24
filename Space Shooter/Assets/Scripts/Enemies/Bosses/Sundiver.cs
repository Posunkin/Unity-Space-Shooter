using UnityEngine;
using System.Collections;

namespace SpaceShooter.Enemies
{
    public class Sundiver : Enemy
    {
        private enum SundiverState
        {
            shooting,
            shock,
            rage
        }

        private SundiverState state;
        private int stateIndex;

        [Header("Sundiver parameters:")]
        [SerializeField] private BossHealthBar healthBar;
        [SerializeField] private EnemyWeaponControl basicWeapons;
        [SerializeField] private EnemyWeaponControl rocketWeapon;
        [SerializeField] private RocketAttack rocketAttack;
        private float shockSpeed = 5;
        private float startSpeed;

        [Header("Defenders parameters:")]
        [SerializeField] private Defender[] defenders;
        [SerializeField] private GameObject sunShield;
        [SerializeField] private float timeToNewDefenders;
        private float defendersDeadTimer;
        private int deadDefenders;

        [Header("Rage parameters:")]
        [SerializeField] private float delayBetweenRocketAttacks;
        private float lastRocketAttack;

        // Movement stats
        private Vector3 p1;
        private float widMinRad;
        private float hgtMinRad;
        private bool isMoving = false;

        private void Start()
        {
            startSpeed = speed;
            state = SundiverState.shooting;
            healthBar.SetMaxHealth(maxHealth);
            stateIndex = 0;
            foreach (Defender def in defenders)
            {
                def.onDefenderDeath += DefenderDead;
                def.Init(maxHealth);
            }
            deadDefenders = 0;
            widMinRad = bndCheck.CamWidth - bndCheck.Radius;
            hgtMinRad = bndCheck.CamHeight - bndCheck.Radius;
            StartCoroutine(nameof(MovementShooting));
            rocketWeapon.Stop();
        }

        public override void Init()
        {
            base.Init();
            healthBar.SetMaxHealth(maxHealth);
        }

        private void ChangeStatus()
        {
            if (stateIndex < 2) stateIndex++;
            else stateIndex = 0;
            state = (SundiverState)stateIndex;
            StopAllCoroutines();
            ReturnColors();
            switch (state)
            {
                case SundiverState.shooting:
                    StartCoroutine(nameof(MovementShooting));
                    break;
                case SundiverState.shock:
                    StartCoroutine(nameof(Stay));
                    break;
                case SundiverState.rage:
                    StartCoroutine(nameof(Raging));
                    break;
                default:
                    StartCoroutine(nameof(MovementShooting));
                    break;
            }
        }
        
        #region Shooting state
        private IEnumerator MovementShooting()
        {
            while (sunShield.activeInHierarchy)
            {
                if (!isMoving)
                {
                    InitMovement();
                }
                MoveShooting();
                yield return null;
            }
            float damageTaken = maxHealth * 0.1f;
            TakeDamage(damageTaken);
            ChangeStatus();
        }

        private Vector3 GetRandomPosition()
        {
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(-widMinRad, widMinRad);
            pos.y = Random.Range(-hgtMinRad, hgtMinRad);
            return pos;
        }

        private void InitMovement()
        {
            Debug.Log("Start moving");
            isMoving = true;
            p1 = GetRandomPosition();
            p1.y = Random.Range(-hgtMinRad + 15, hgtMinRad);
            Debug.Log(p1);
        }

        private void MoveShooting()
        {
            transform.position = Vector3.MoveTowards(transform.position, p1, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, p1) < 0.1f)
            {
                isMoving = false;
                InitMovement();
            }
        }
        #endregion

        #region Shock state
        private IEnumerator Stay()
        {
            speed = shockSpeed;
            float startedState = Time.time;
            basicWeapons.Stop();
            rocketWeapon.Activate();
            while (Time.time - startedState < timeToNewDefenders)
            {
                MoveUp();
                yield return null;
            }
            rocketWeapon.Stop();
            basicWeapons.Activate();
            speed = startSpeed;
            ChangeStatus();
        }

        private void MoveUp()
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, hgtMinRad, 0), speed * Time.deltaTime);
        }
        #endregion

        #region Rage state
        private IEnumerator Raging()
        {
            float startedState = Time.time;
            while (Time.time - startedState < timeToNewDefenders)
            {
                if (!isMoving)
                {
                    InitMovement();
                }
                MoveShooting();
                ShotRockets();
                yield return null;
            }
            NewDefenders();
            ChangeStatus();
        }

        private void ShotRockets()
        {
            if (Time.time - lastRocketAttack < delayBetweenRocketAttacks) return;
            lastRocketAttack = Time.time;
            int index = Random.Range(3, 8);
            for (int i = 0; i <= index; i++)
            {
                Vector3 targetPos = GetRandomPosition();
                rocketAttack.Attack(targetPos);
            }
        }
        #endregion
        #region Defenders logic
        private void DefenderDead()
        {
            deadDefenders++;
            if (deadDefenders == defenders.Length)
            {
                sunShield.SetActive(false);
                defendersDeadTimer = Time.time;
            }
        }

         public override void TakeDamage()
        {
            if (sunShield.activeInHierarchy) return;
            base.TakeDamage();
            healthBar.ChangeHealth(currentHealth);
        }
        
        public override void TakeDamage(float damage)
        {
            if (sunShield.activeInHierarchy) return;
            base.TakeDamage(damage);
            healthBar.ChangeHealth(currentHealth);
        }

        private void NewDefenders()
        {
            deadDefenders = 0;
            foreach (Defender def in defenders)
            {
                def.gameObject.SetActive(true);
                def.Alive(maxHealth);
            }
            sunShield.SetActive(true);
        }
        #endregion
    }
}

