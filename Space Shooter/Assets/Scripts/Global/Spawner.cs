using System;
using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private GameObject powerUpPrefab;
    [SerializeField] private float startDelay = 2;
    [SerializeField] private float spawnDelay = 2;
    internal List<GameObject> enemiesOnScene = new();
    private float _offSet = 2;

    internal float OffSet { get => _offSet; private set => _offSet = value; }

    private void Start()
    {
        Invoke(nameof(SpawnEnemies), startDelay);
    }

    private void SpawnEnemies()
    {
        int index = Random.Range(0, enemiesPrefabs.Length);
        GameObject go = Instantiate(enemiesPrefabs[index]);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.OnDeath += SpawnPowerUp;

        // Set the position of the new object
        go.transform.position = SetupPosition(go);
        enemiesOnScene.Add(go);

        Invoke(nameof(SpawnEnemies), spawnDelay);
    }

    private Vector3 SetupPosition(GameObject go)
    {
        BoundsCheck enemyBnd = go.GetComponent<BoundsCheck>();
        
        float enemyOffSet = OffSet;
        if (enemyBnd != null)
        {
            enemyOffSet = Mathf.Abs(enemyBnd.Radius);
        }

         // Set the starting coordinates
        Vector3 pos = Vector3.zero;
        pos.x = Random.Range(-enemyBnd.CamWidth + enemyOffSet, enemyBnd.CamWidth - enemyOffSet);
        pos.y = enemyBnd.CamHeight + enemyOffSet;
        return pos;
    }

    public void SpawnPowerUp(Enemy enemy)
    {
        enemy.OnDeath -= SpawnPowerUp;
        float chance = enemy.chanceToSpawnPowerUp;
        if (chance > Random.Range(0f, 1f))
        {
            GameObject go = Instantiate(powerUpPrefab);
            go.transform.position = enemy.gameObject.transform.position;
            WeaponPowerUp pwr = powerUpPrefab.GetComponent<WeaponPowerUp>();
            int index = Random.Range(0, pwr.weaponTypeFrequency.Length);
            WeaponType type = pwr.GetWeaponType(index);
            go.GetComponent<PowerUp>().SetType(type);
            Debug.Log(type);
        }
    }
}
