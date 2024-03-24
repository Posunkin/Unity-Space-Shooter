using System.Collections;
using SpaceShooter.Enemies;
using UnityEngine;

public class LaserAttack : MonoBehaviour
{
    [Header("Boss Laser parameters:")]
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float laserWidth = 0.4f;
    [SerializeField] private float maxLength = 300f;
    [SerializeField] private LayerMask hitLayer;
    private Transform tr;
    private float duration = 3;
    private float attackStart;
    private float lastShootTime;
    private float delayBetweenShots = 5;
    private WaitForSeconds attackDelay = new WaitForSeconds(0.2f);

    private void Start()
    {
        tr = transform;
        lineRenderer.startWidth = laserWidth;
        lineRenderer.endWidth = laserWidth;
        lineRenderer.enabled = false;
    }

    private void FixedUpdate()
    {
        lineRenderer.SetPosition(0, new Vector3(tr.position.x, tr.position.y, 0));
        lineRenderer.SetPosition(1, tr.position + Vector3.down * maxLength);
    }

    public void Attack()
    {
        if (Time.time - lastShootTime < delayBetweenShots) return;
        lastShootTime = Time.time;
        StartCoroutine(nameof(Shoot));
    }

    private IEnumerator Shoot()
    {
        lineRenderer.enabled = true;
        attackStart = Time.time;
        while (Time.time - attackStart < duration)
        {
            DoDamage();
            yield return attackDelay;
        }
        lineRenderer.enabled = false;
    }

    private void DoDamage()
    {
        RaycastHit hit;
        Physics.Raycast(new Vector3(tr.position.x, tr.position.y, 0), Vector3.down, out hit, maxLength, hitLayer);

        if (hit.collider != null)
        {
            Transform parent = hit.collider.transform.root;
            
            if (parent.TryGetComponent<PlayerStats>(out PlayerStats player))
            {
                player.TakeDamage();
            }
            else if (parent.TryGetComponent<Enemy>(out Enemy enemy))
            {
                enemy.TakeDamage();
            }
        }
    }
}
