using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Enemy_Killer : Enemy
    {
        [SerializeField] private float lifeTime;
        private Vector3 p1;
        private float widMinRad;
        private float hgtMinRad;
        private bool isMoving = false;
        private bool isLeaving = false;
        private float birthTime;
        
        private void Start()
        {
            birthTime = Time.time;
            widMinRad = bndCheck.CamWidth - bndCheck.Radius;
            hgtMinRad = bndCheck.CamHeight - bndCheck.Radius;
        }

        private void Update()
        {
            if (Time.time - birthTime > lifeTime)
            {
                if (!isLeaving)
                {
                    if (transform.position.x < 0)
                    {
                        p1.x = -widMinRad - bndCheck.Radius - 15;
                    }
                    else
                    {
                        p1.x = widMinRad + bndCheck.Radius + 15;
                    }
                    isLeaving = true;
                }
                MoveOutOfBounds();
                return;
            }
            if (!isMoving) InitMovement();
            Move();
        }

        protected override void Move()
        {
            transform.position = Vector3.MoveTowards(transform.position, p1, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, p1) < 0.1f)
            {
                isMoving = false;
            }
        }

        private void InitMovement()
        {
            isMoving = true;
            p1 = GetRandomPosition();
        }
        
        private Vector3 GetRandomPosition()
        {
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(-widMinRad, widMinRad);
            pos.y = Random.Range(-hgtMinRad + 15, hgtMinRad);
            return pos;
        }

        private void MoveOutOfBounds()
        {
            transform.position = Vector3.MoveTowards(transform.position, p1, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, p1) < 0.1f)
            {
                Death(false);
            }
        }
    }
}

