using System.Collections;
using SpaceShooter.Enemies;
using UnityEngine;

namespace SpaceShooter.Weapons
{
    public class LaserGun : Weapon
    {
        [Header("Laser Gun parameters:")]
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private float laserWidth = 0.3f;
        [SerializeField] private float maxLength = 300f;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private ParticleSystem startParticles;
        private Transform tr;
        private WaitForSeconds shotDuration = new WaitForSeconds(0.15f);
        
        protected override void Start()
        {
            type = WeaponType.laserGun;
            tr = transform;
            weaponControl.OnWeaponShoot += TempFire;
            lineRenderer.startWidth = laserWidth;
            lineRenderer.endWidth = laserWidth;
            lineRenderer.enabled = false;
        }

        protected override void TempFire()
        {
            if (lastShootTime + delayBetweenShots > Time.time) return;
            lastShootTime = Time.time;
            StartCoroutine(nameof(Shooting));
        }

        private void FixedUpdate()
        {
            lineRenderer.SetPosition(0, new Vector3(tr.position.x, tr.position.y, 0));
            lineRenderer.SetPosition(1, tr.position + Vector3.up * maxLength);
        }

        private IEnumerator Shooting()
        {
            startParticles.Play();
            for (int i = 0; i < 7; i++)
            {
                DoDamage();
                yield return shotDuration;
            }
            lineRenderer.enabled = false;
            startParticles.Stop();
        }

        private void DoDamage()
        {
            lineRenderer.enabled = true;
            RaycastHit[] hits = Physics.RaycastAll(new Vector3(tr.position.x, tr.position.y, 0), Vector3.up,maxLength,enemyLayer);
            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {
                    Transform parent = hit.collider.transform.root;
                    Enemy enemy = parent.GetComponent<Enemy>();
                        if (enemy != null)
                        {
                            enemy.TakeDamage(currentDamage);
                        }
                }
            }
        }

        protected override GameObject Shoot()
        {
            return null;
        }
    }
}