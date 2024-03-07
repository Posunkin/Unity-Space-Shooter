using SpaceShooter.Enemies;
using UnityEngine;

public class MineProjectile : Projectile
{
    [SerializeField] private float explosionRadius;
    [SerializeField] private float timeToExplose = 3;
    [SerializeField] private GameObject expEffect;
    [SerializeField] private GameObject radiusVisual;

    private Renderer radiusRend;
    private bool isPlayer;
    private float currentTime = 0f;
    
    private void Start()
    {
        radiusRend = radiusVisual.GetComponent<Renderer>();
    }   

    internal void SetProjectile(float damage, float timeToExplose, float explosionRadius, bool isPlayer)
    {
        damageToDeal = damage;
        this.explosionRadius = explosionRadius;
        this.timeToExplose = timeToExplose;
        this.isPlayer = isPlayer;
    }

    protected override void Update()
    {   
        currentTime += Time.deltaTime;
        base.Update();
        float explosionProgress = Mathf.Clamp01(currentTime / timeToExplose);

        Color color = radiusRend.material.color;
        color.a = 1f - explosionProgress;
        radiusRend.material.color = color;
        radiusVisual.transform.localScale = Vector3.one * (1f + explosionProgress) * explosionRadius;

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
