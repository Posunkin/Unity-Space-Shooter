using System;
using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemiesPrefabs;
    [SerializeField] private GameObject[] bossesPrefabs;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private GameObject weaponPowerUpPrefab;
    [SerializeField] private GameObject playerPowerUpPrefab;
    [SerializeField] private float startDelay = 2;
    [SerializeField] private float spawnDelay = 2;
    [SerializeField] private float delayBetweenBosses;
    private float timeSinceLastBoss;
    private int bossIndex = 0;
    internal List<GameObject> enemiesOnScene = new();
    private float _offSet = 2;

    internal float OffSet { get => _offSet; private set => _offSet = value; }

    [Header("For Stars Eater:")]
    [SerializeField] private GameObject[] starEaterTeam;

    private void Start()
    {
        timeSinceLastBoss = Time.time;
        Invoke(nameof(SpawnEnemies), startDelay);
        // Invoke(nameof(SpawnBoss), startDelay);
    }

    private void SpawnEnemies()
    {
        if (Time.time - timeSinceLastBoss > delayBetweenBosses)
        {
            SpawnBoss();
            return;
        }
        int index = Random.Range(0, enemiesPrefabs.Length);
        GameObject go = Instantiate(enemiesPrefabs[index]);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.OnDeath += SpawnPowerUp;

        // Set the position of the new object
        go.transform.position = SetupPosition(go);
        Debug.Log(go.transform.position);
        enemiesOnScene.Add(go);

        Invoke(nameof(SpawnEnemies), spawnDelay);
    }

    private void SpawnBoss()
    {
        GameObject go = Instantiate(bossesPrefabs[bossIndex]);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.OnDeath += BossDead;

        // Set the position of the new object
        go.transform.position = bossPosition.position;
        enemiesOnScene.Add(go);
        if (bossIndex < bossesPrefabs.Length - 1)
        {
            bossIndex++;
        }
        else
        {
            bossIndex = 0;
        }
    }

    public void SpawnBossTeam()
    {
        int index = Random.Range(0, starEaterTeam.Length);
        GameObject go = Instantiate(starEaterTeam[index]);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.OnDeath += SpawnPowerUp;

        go.transform.position = SetupPosition(go);
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
        pos.y = enemyBnd.CamHeight + enemyOffSet * 2.5f;
        return pos;
    }

    private void BossDead(Enemy boss)
    {
        boss.OnDeath -= BossDead;
        for (int i = 0; i < 3; i++)
        {
            SpawnPowerUp(PowerUpType.shield, boss.transform.position);
        }
        timeSinceLastBoss = Time.time;
        Invoke(nameof(SpawnEnemies), 4);
    }

    public void SpawnPowerUp(Enemy enemy)
    {
        enemy.OnDeath -= SpawnPowerUp;
        float chance = enemy.chanceToSpawnPowerUp;
        if (chance > Random.Range(0f, 1f))
        {
            if (0.5 > Random.Range(0f, 1f)) SpawnWeaponPowerUp(enemy);
            else SpawnPlayerPowerUp(enemy);
        }
    }

    private void SpawnPlayerPowerUp(Enemy enemy)
    {
        GameObject go = Instantiate(playerPowerUpPrefab);
        go.transform.position = enemy.gameObject.transform.position;
        PlayerPowerUp pwr = playerPowerUpPrefab.GetComponent<PlayerPowerUp>();
        int index = Random.Range(0, pwr.powerUpTypeFrequency.Length);
        PowerUpType type = pwr.GetPowerUpType(index);
        go.GetComponent<PlayerPowerUp>().SetType(type);
    }

    private void SpawnWeaponPowerUp(Enemy enemy)
    {
        GameObject go = Instantiate(weaponPowerUpPrefab);
        go.transform.position = enemy.gameObject.transform.position;
        WeaponPowerUp pwr = weaponPowerUpPrefab.GetComponent<WeaponPowerUp>();
        int index = Random.Range(0, pwr.weaponTypeFrequency.Length);
        WeaponType type = pwr.GetWeaponType(index);
        go.GetComponent<WeaponPowerUp>().SetType(type);
    }

    private void SpawnPowerUp(PowerUpType type, Vector3 position)
    {
        GameObject go = Instantiate(playerPowerUpPrefab);
        go.transform.position = position;
        go.GetComponent<PlayerPowerUp>().SetType(type);
    }
}
