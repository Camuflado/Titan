using UnityEngine;
using System.Collections;

public class BossSpawn : MonoBehaviour
{

    public Wave[] waves;
    public Enemy FireBall; //referencia ao inimigo para spawnar

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    void Start()
    {
        NextWave();
    }

    void Update()
    {

        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(FireBall, GameObject.Find("BossFire").transform.position, Quaternion.identity) as Enemy;
            //Enemy spawnedEnemy = Instantiate(FireBall, Vector3.zero, Quaternion.identity) as Enemy; //Quaternion = particular rotation
            spawnedEnemy.OnDeath += OnEnemyDeath; //Contar Mortes
        }
    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if (enemiesRemainingAlive == 0)
        {
            NextWave(); //Começar próxima wave
        }
    }

    void NextWave()
    {
        currentWaveNumber++;
        //print("Wave: " + currentWaveNumber);
        if (currentWaveNumber - 1 < waves.Length) //If para não dar exception se não houver mais waves
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
