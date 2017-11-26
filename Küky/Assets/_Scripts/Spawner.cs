using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Wave[] waves;
    public Enemy enemy;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainAlive;
    float nextSpawnTime;

    void Start(){
        NextWave();
    }

    void Update(){
        if(enemiesRemainingToSpawn>0 && Time.time>nextSpawnTime){
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            Enemy spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity) as Enemy;
            spawnedEnemy.OnDeath += OnEnemyDeath;
        }
    }

    void OnEnemyDeath(){
        enemiesRemainAlive--;
        if(enemiesRemainAlive == 0){
            NextWave();
        }
       // print("Enemies died");
    }

    void NextWave(){
        currentWaveNumber++;
        print("Wave: "+currentWaveNumber);
        if(currentWaveNumber-1<waves.Length){
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount;
            enemiesRemainAlive = enemiesRemainingToSpawn;
        }
       
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount;
        public float timeBetweenSpawns;
    }
}
