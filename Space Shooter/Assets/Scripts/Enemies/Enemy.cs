using System;
using UnityEngine;

namespace SpaceShooter.Enemies
{
    public abstract class Enemy : MonoBehaviour, IDamageable
    {
        public Action<Enemy> OnDeath;
        [SerializeField] internal float health;
        [SerializeField] internal float speed;
        [SerializeField] internal readonly float score;
        internal Vector3 pos;
        internal BoundsCheck bndCheck;

        internal abstract void Move();
        internal abstract void CheckBounds();
        internal abstract void OnTriggerEnter(Collider other);
        internal abstract void OnCollisionEnter(Collision other);
    }
}
