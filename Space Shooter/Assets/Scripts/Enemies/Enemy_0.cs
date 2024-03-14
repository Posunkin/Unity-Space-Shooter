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
            if (bndCheck != null && bndCheck.offDown)
            {
                Death();
            }
        }
    }
}
