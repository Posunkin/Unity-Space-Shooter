using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Enemy_Gunshoot : Enemy
    {
        private float lifeTime = 5;

        private Vector3[] points;
        private float birthTime;

        private void Start()
        {
            points = new Vector3[3];
            points[0] = pos;

            float xMin = -bndCheck.CamWidth + bndCheck.Radius;
            float xMax = bndCheck.CamWidth - bndCheck.Radius;

            Vector3 v;
            v = Vector3.zero;
            v.x = Random.Range(xMin, xMax);
            v.y = -bndCheck.CamHeight * Random.Range(2.75f, 2);
            points[1] = v;

            v = Vector3.zero;
            v.y = pos.y;
            v.x = Random.Range(xMin, xMax);
            points[2] = v;

            birthTime = Time.time;
        }

        private void Update()
        {
            Move();
        }

        protected override void Move()
        {
            float u = (Time.time - birthTime) / lifeTime;

            if (u > 1)
            {
                Death(false);
                return;
            }

            Vector3 p01, p12;
            // u = u - 0.2f * Mathf.Sin(u * Mathf.PI * 2);
            p01 = (1 - u) * points[0] + u * points[1];
            p12 = (1 - u) * points[1] + u * points[2];
            pos = (1 - u) * p01 + u * p12;
        }
    }
}

