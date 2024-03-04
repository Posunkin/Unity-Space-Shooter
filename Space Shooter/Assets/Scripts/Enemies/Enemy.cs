using System;
using UnityEngine;

namespace SpaceShooter.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        public Action<Enemy> OnDeath;
        [SerializeField] protected float health;
        [SerializeField] protected float speed;
        [SerializeField] protected readonly float score;
        protected float damageOnTrigger = 10;
        protected Vector3 pos {get => this.transform.position; set => this.transform.position = value; }
        protected Vector3 tempPos;
        protected BoundsCheck bndCheck;

        protected virtual void Move()
        {
            tempPos = pos;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
        protected abstract void CheckBounds();
        protected abstract void OnCollisionEnter(Collision other);
        protected abstract void OnTriggerEnter(Collider other);
    }
}
