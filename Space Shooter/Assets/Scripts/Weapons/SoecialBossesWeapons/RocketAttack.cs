using System.Collections;
using UnityEngine;

public class RocketAttack : MonoBehaviour
{
    [SerializeField] private GameObject explosionVisual;
    [SerializeField] private float explosionRadius;
    [SerializeField] private GameObject rocketProjectile;
    [SerializeField] private float timeToShot;
    [SerializeField] private float speed;
    [SerializeField] private Transform ProjectileAnchor;
    private WaitForSeconds waitToShot;

    public void Attack(Vector3 position)
    {
        GameObject go = Instantiate(explosionVisual);
        go.transform.position = position;
        go.transform.localScale = Vector3.one * explosionRadius;
        waitToShot = new(timeToShot);
        StartCoroutine(nameof(ShotRocket), go.transform);
    }

    private IEnumerator ShotRocket(Transform target)
    {
        yield return waitToShot;
        GameObject go = Instantiate(rocketProjectile, ProjectileAnchor);
        SundiverRocket rocket = go.GetComponent<SundiverRocket>();
        rocket.SetProjectile(explosionRadius, speed, false, target);
        go.transform.position = transform.position;
    }
}
