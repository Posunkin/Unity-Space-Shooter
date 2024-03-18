using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SpaceShooter.Enemies
{
    public class Stars_Eater : Enemy
    {
        private enum StarsEaterState
        {
            shooting,
            chasingLaser,
            laserAttack
        }

        [SerializeField] private float shootStatusTime;
        [SerializeField] private float chasingLaserStatusTime;
        [SerializeField] private float laserAttackStatusTime;
        [SerializeField] private EnemyWeaponControl weapControl;
        [SerializeField] private BossHealthBar healthBar;
        private Spawner spawner;
        private float startedNewStatusTime;
        private int stateIndex = 0;
        // Shooting movement stats
        private Vector3 p0, p1;
        private float timeStart;
        private float duration = 5f;
        private float widMinRad;
        private float hgtMinRad;
        private bool isMoving = false;

        // Chasing laser stats
        [SerializeField] private LaserAttack laserAttack;
        [SerializeField] private ParticleSystem chargingEffect;
        private bool endOfScreen = true;
        private float x = 0;
        private float speedWhileAttack = 5;
        private float startSpeed;

        private StarsEaterState state;

        private void Start()
        {
            weapControl = GetComponent<EnemyWeaponControl>();
            healthBar.SetMaxHealth(maxHealth);
            state = StarsEaterState.shooting;
            spawner = GameObject.Find("Spawner").GetComponent<Spawner>();
            p0 = p1 = pos;
            StartCoroutine(nameof(Shooting));
            widMinRad = bndCheck.CamWidth - bndCheck.Radius;
            hgtMinRad = bndCheck.CamHeight - bndCheck.Radius;
            startSpeed = speed;
        }

        private void ChangeStatus()
        {
            if (stateIndex < 2) stateIndex++;
            else stateIndex = 0;
            state = (StarsEaterState)stateIndex;
            StopAllCoroutines();
            switch (state)
            {
                case StarsEaterState.shooting:
                    StartCoroutine(nameof(Shooting));
                    break;
                case StarsEaterState.chasingLaser:
                    StartCoroutine(nameof(ChasingLaser));
                    break;
                case StarsEaterState.laserAttack:
                    StartCoroutine(nameof(LaserGo));
                    break;
            }
        }

        public override void TakeDamage(float damage)
        {
            currentHealth -= damage;
            healthBar.ChangeHealth(currentHealth);
            StartCoroutine(nameof(ShowDamage));
            if (currentHealth <= 0)
            {
                GameManager.Instance.UpdateScore(score);
                Death();
            }
            else if (currentHealth <= maxHealth / 2)
            {
                smokeEffect.Play();
            }
        }

        #region Methods for Shooting status
        private IEnumerator Shooting()
        {
            startedNewStatusTime = Time.time;
            while (Time.time - startedNewStatusTime < shootStatusTime)
            {
                if (!isMoving)
                {
                    Debug.Log("Start moving");
                    isMoving = true;
                    p0 = transform.position;
                    p1 = transform.position;
                    p1.x = Random.Range(-widMinRad, widMinRad);
                    p1.y = Random.Range(-hgtMinRad + 10, hgtMinRad);
                    timeStart = Time.time;
                }
                MoveShooting();
                yield return null;
            }
            ChangeStatus();
        }

        private void MoveShooting()
        {
            if (Time.time - timeStart > duration)
            {
                isMoving = false;
            }
            Vector3 movePos = transform.position;
            movePos += p1.normalized * speed * Time.deltaTime;
            transform.position = movePos;
        }
        #endregion

        #region Methods for Chasing laser status
        private IEnumerator ChasingLaser()
        {
            InvokeRepeating(nameof(SpawnBossTeam), 1, 3.5f);
            isMoving = false;
            chargingEffect.Play();
            Debug.Log("Start chasing laser");
            startedNewStatusTime = Time.time;
            while (Time.time - startedNewStatusTime < chasingLaserStatusTime)
            {
                MoveWithLaser();
                yield return null;
            }
            chargingEffect.Stop();
            ChangeStatus();
        }

        private void MoveWithLaser()
        {
            Vector3 movePos = transform.position;
            while (transform.position.y < hgtMinRad)
            {
                movePos.y += speed * Time.deltaTime;
                transform.position = movePos;
                return;
            }
            if (endOfScreen) 
            {
                x = transform.position.x;
                endOfScreen = false;
            }
            if (x < 0)
            {
                movePos.x += speed * Time.deltaTime;
                transform.position = movePos;
                if (transform.position.x > widMinRad)
                {
                    endOfScreen = true;
                }
            }
            else
            {
                movePos.x -= speed * Time.deltaTime;
                transform.position = movePos;
                if (transform.position.x < -widMinRad)
                {
                    endOfScreen = true;
                }
            }
        }

        #endregion
        
        #region  Methods for Laser attack status
        private IEnumerator LaserGo()
        {
            weapControl.Stop();
            speed = speedWhileAttack;
            Debug.Log("Start attacking!");
            startedNewStatusTime = Time.time;
            while (Time.time - startedNewStatusTime < laserAttackStatusTime)
            {
                MoveWithLaser();
                laserAttack.Attack();
                yield return null;
            }
            state = StarsEaterState.shooting;
            speed = startSpeed;
            weapControl.Activate();
            CancelInvoke(nameof(SpawnBossTeam));
            ChangeStatus();
        }

        private void SpawnBossTeam()
        {
            spawner.SpawnBossTeam();
        }
        #endregion
    }
}

