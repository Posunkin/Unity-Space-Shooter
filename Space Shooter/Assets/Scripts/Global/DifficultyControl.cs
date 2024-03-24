using SpaceShooter.Enemies;

public class DifficultyControl
{
    private float hpMult = 0.8f;
    private float spawnDelay = 1.6f;
    private float speedMult = 0.8f;
    private float maxEnemiesOnScreen = 4;
    private float chanceToSpawnPowerUp = 0.5f;
    private int currentLevel = 0;
    
    public float SpawnDelay {get => spawnDelay; private set => spawnDelay = value;}
    public float MaxEnemiesOnScreen {get => maxEnemiesOnScreen; private set => maxEnemiesOnScreen = value;}
    public int CurrentLevel {get => currentLevel; private set => currentLevel = value;}
   

    public void DifficultyChange()
    {
        if (currentLevel < 4) currentLevel++;
        hpMult += 0.2f;
        if (spawnDelay > 0.8f) spawnDelay -= 0.15f;
        if (speedMult < 1.6f) speedMult += 0.2f;
        if (chanceToSpawnPowerUp > 0.1f)
        {
            chanceToSpawnPowerUp -= 0.1f;
        }
        if (maxEnemiesOnScreen < 10) maxEnemiesOnScreen++;
    }

    public void UpEnemy(Enemy enemy)
    {
        enemy.MaxHealth *= hpMult;
        enemy.Speed *= speedMult;
        enemy.ChanceToSpawnPowerUp = chanceToSpawnPowerUp;
    }
}
