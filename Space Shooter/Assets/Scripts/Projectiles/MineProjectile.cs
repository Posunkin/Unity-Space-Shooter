using SpaceShooter.Enemies;
using UnityEngine;

public class MineProjectile : Projectile
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject explosionVisual;
    [SerializeField] private float timeToExplose = 3;
    [SerializeField] private GameObject expEffect;
    private Material radiusMat;
    private Renderer radiusRenderer;

    private bool isPlayer;
    private float currentTime = 0f;
    

    internal void SetProjectile(float damage, float timeToExplose, float explosionRadius, bool isPlayer)
    {
        damageToDeal = damage;
        this.explosionRadius = explosionRadius;
        this.timeToExplose = timeToExplose;
        this.isPlayer = isPlayer;
    }

    private void Start()
    {
        radiusRenderer = explosionVisual.GetComponent<Renderer>();
        radiusMat = radiusRenderer.material;
    }

    protected override void Update()
    {   
        currentTime += Time.deltaTime;
        float explosionProgress = Mathf.Clamp01(currentTime / timeToExplose);
        Color color = radiusRenderer.material.color;
        color.a = explosionProgress + 1;
        radiusRenderer.material.color = color;
        explosionVisual.transform.localScale = Vector3.one * (1f + explosionProgress) * explosionRadius;
        base.Update();

        if (currentTime >= timeToExplose)
        {
            Explode();
        }
    }

    private void Explode()
    {
        GameObject go = Instantiate(expEffect, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider collider in colliders)
        {
            if (!isPlayer)
            {
                PlayerStats player = collider.GetComponentInParent<PlayerStats>();
                if (player != null)  player.TakeDamage();
            }
            else
            {
                Debug.Log(collider.transform.root.gameObject.name);
                Enemy enemy = collider.GetComponentInParent<Enemy>();
                if (enemy != null) enemy.TakeDamage(damageToDeal);
            }
        }

        Destroy(gameObject);
    }
}
