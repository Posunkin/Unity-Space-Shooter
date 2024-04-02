using UnityEngine;

namespace SpaceShooter.Enemies
{
    public class Enemy_0 : Enemy
    {
        private void Update()
        {
            base.Move();
            CheckBounds();
        }

        private void CheckBounds()
        {
            if (bndCheck != null && transform.position.y < -bndCheck.CamHeight + (bndCheck.Radius * 2))
            {
                Death(false);
            }
        }
    }
}
