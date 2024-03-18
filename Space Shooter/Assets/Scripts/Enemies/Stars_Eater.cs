using System.Collections;
using UnityEngine;

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

        [SerializeField] private float changeStatusTime = 10f;
        [SerializeField] private EnemyWeaponControl weapControl;
        [SerializeField] private Spawner spawner;
        private float startedNewStatusTime;
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
            state = StarsEaterState.shooting;
            p0 = p1 = pos;
            StartCoroutine(nameof(Shooting));
            widMinRad = bndCheck.CamWidth - bndCheck.Radius;
            hgtMinRad = bndCheck.CamHeight - bndCheck.Radius;
            startSpeed = speed;
        }

        // Methods for Shooting status
        private IEnumerator Shooting()
        {
            startedNewStatusTime = Time.time;
            while (Time.time - startedNewStatusTime < changeStatusTime)
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
            state = StarsEaterState.chasingLaser;
            StartCoroutine(nameof(ChasingLaser));
            StopCoroutine(nameof(Shooting));
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

        // Methods for Chasing laser status
        private IEnumerator ChasingLaser()
        {
            InvokeRepeating(nameof(SpawnBossTeam), 1, 3.5f);
            isMoving = false;
            chargingEffect.Play();
            Debug.Log("Start chasing laser");
            startedNewStatusTime = Time.time;
            while (Time.time - startedNewStatusTime < changeStatusTime)
            {
                MoveWithLaser();
                yield return null;
            }
            chargingEffect.Stop();
            state = StarsEaterState.laserAttack;
            StartCoroutine(nameof(LaserGo));
            StopCoroutine(nameof(ChasingLaser));
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
    
        // Methods for Laser attack status
        private IEnumerator LaserGo()
        {
            weapControl.Stop();
            speed = speedWhileAttack;
            Debug.Log("Start attacking!");
            startedNewStatusTime = Time.time;
            while (Time.time - startedNewStatusTime < changeStatusTime)
            {
                MoveWithLaser();
                laserAttack.Attack();
                yield return null;
            }
            state = StarsEaterState.shooting;
            speed = startSpeed;
            weapControl.Activate();
            CancelInvoke(nameof(SpawnBossTeam));
            StartCoroutine(nameof(Shooting));
            StopCoroutine(nameof(ChasingLaser));
        }

        private void SpawnBossTeam()
        {
            spawner.SpawnBossTeam();
        }
    }
}

