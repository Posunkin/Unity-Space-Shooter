using System;
using System.Collections;
using System.Collections.Generic;
using SpaceShooter.Enemies;
using UnityEngine;
using Random = UnityEngine.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject[][] enemiesPrefabs = new GameObject[4][];
    [SerializeField] private GameObject[] enemiesPrefabsLevel1;
    [SerializeField] private GameObject[] enemiesPrefabsLevel2;
    [SerializeField] private GameObject[] enemiesPrefabsLevel3;
    [SerializeField] private GameObject[] enemiesPrefabsLevel4;
    [SerializeField] private GameObject[] bossesPrefabs;
    [SerializeField] private Transform bossPosition;
    [SerializeField] private GameObject weaponPowerUpPrefab;
    [SerializeField] private GameObject playerPowerUpPrefab;
    [SerializeField] private float startDelay = 2;
    [SerializeField] private float delayBetweenBosses;
    private DifficultyControl difficultyControl;
    private float timeSinceLastBoss;
    private int bossIndex = 0;
    internal int enemiesOnScene = 0;
    private float _offSet = 2;

    internal float OffSet { get => _offSet; private set => _offSet = value; }

    [Header("For Bosses:")]
    [SerializeField] private GameObject[] starEaterTeam;
    [SerializeField] private GameObject[] spaceKeeperTeam;

    

    private void Start()
    {
        enemiesPrefabs[0] = enemiesPrefabsLevel1;
        enemiesPrefabs[1] = enemiesPrefabsLevel2;
        enemiesPrefabs[2] = enemiesPrefabsLevel3;
        enemiesPrefabs[3] = enemiesPrefabsLevel4;
        difficultyControl = new();
        timeSinceLastBoss = Time.time;
        Invoke(nameof(SpawnEnemies), startDelay);
        // Invoke(nameof(SpawnBoss), startDelay);
    }

    private void SpawnEnemies()
    {
        if (Time.time - timeSinceLastBoss > delayBetweenBosses)
        {
            Debug.Log("Spawn boss" + bossesPrefabs[bossIndex].gameObject.name);
            SpawnBoss();
            return;
        }
        if (enemiesOnScene == difficultyControl.MaxEnemiesOnScreen)
        {
            Invoke(nameof(SpawnEnemies), difficultyControl.SpawnDelay);
            return;
        }
        int index = Random.Range(0, enemiesPrefabs[difficultyControl.CurrentLevel].Length);
        GameObject go = Instantiate(enemiesPrefabs[difficultyControl.CurrentLevel][index]);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.OnDeath += SpawnPowerUp;
        difficultyControl.UpEnemy(enemy);
        enemy.Init();

        // Set the position of the new object
        go.transform.position = SetupPosition(go);
        Debug.Log(go.transform.position);
        enemiesOnScene++;
        Debug.Log(enemiesOnScene);

        Invoke(nameof(SpawnEnemies), difficultyControl.SpawnDelay);
    }

    private void SpawnBoss()
    {
        GameManager.Instance.BossMusic();
        GameObject go = Instantiate(bossesPrefabs[bossIndex]);
        Enemy enemy = go.GetComponent<Enemy>();
        enemy.OnDeath += BossDead;
        difficultyControl.UpEnemy(enemy);
        enemy.Init();

        difficultyControl.DifficultyChange();
        // Set the position of the new object
        go.transform.position = bossPosition.position;
        enemiesOnScene++;
        if (bossIndex < bossesPrefabs.Length - 1)
        {
            bossIndex++;
        }
        else
        {
            bossIndex = 0;
        }
    }

    public void SpawnBossTeam(string bossName)
    {
        int index;
        GameObject go;
        Enemy enemy;
        switch (bossName)
        {
            case "Stars_Eater":
                index = Random.Range(0, starEaterTeam.Length);
                go = Instantiate(starEaterTeam[index]);
                enemy = go.GetComponent<Enemy>();
                enemy.OnDeath += SpawnPowerUp;
                go.transform.position = SetupPosition(go);
                break;
            case "Space_Keeper":
                index = Random.Range(0, spaceKeeperTeam.Length);
                go = Instantiate(spaceKeeperTeam[index]);
                enemy = go.GetComponent<Enemy>();
                enemy.OnDeath += SpawnPowerUp;
                go.transform.position = SetupPosition(go);
                break;
        }
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

    private void BossDead(Enemy boss, bool fromPlayer)
    {
        boss.OnDeath -= BossDead;
        enemiesOnScene--;
        for (int i = 0; i < 2; i++)
        {
            SpawnPowerUp(PowerUpType.shield, boss.transform.position);
        }
        SpawnPowerUp(PowerUpType.damage, boss.transform.position);
        timeSinceLastBoss = Time.time;
        Invoke(nameof(SpawnEnemies), 4);
    }

    public void SpawnPowerUp(Enemy enemy, bool fromPlayer)
    {
        enemy.OnDeath -= SpawnPowerUp;
        enemiesOnScene--;
        if (fromPlayer)
        {
            float chance = enemy.chanceToSpawnPowerUp;
            if (chance > Random.Range(0f, 1f))
            {
                if (0.5 > Random.Range(0f, 1f)) SpawnWeaponPowerUp(enemy);
                else SpawnPlayerPowerUp(enemy);
            }
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
