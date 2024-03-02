using UnityEngine;

namespace SpaceShooter.Enemies
{
    public abstract class Enemy : MonoBehaviour
    {
        [SerializeField] internal float speed;
        [SerializeField] internal float score;
        internal Vector3 pos;
        internal BoundsCheck bndCheck;

        internal abstract void Move();
        internal abstract void CheckBounds();
        internal abstract void OnTriggerEnter(Collider other);
    }
}
