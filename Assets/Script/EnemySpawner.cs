using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject[] enemySpawnList;

    public List<SpawnedEnemy> spawnableEnemies = new();

    private void Start()
    {
        InvokeRepeating("EnemySpawn", 2.5f, 2.5f);
    }

    void EnemySpawn()
    {
        var enemyToSpawn = SpawnedEnemyCheck();
        if (enemyToSpawn == null) return;

        var spawnPointOrigin = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Vector2 spawnPoint = Vector2.zero;

        if (spawnPointOrigin == spawnPoints[0] || spawnPointOrigin == spawnPoints[1])
        {
            spawnPoint = new Vector2(spawnPointOrigin.position.x + Random.Range(-6.2f, 6.2f), spawnPointOrigin.position.y);
        }
        else
        {
            spawnPoint = new Vector2(spawnPointOrigin.position.x, spawnPointOrigin.position.y + Random.Range(-4.5f, 4.5f));
        }

        var spawnedEnemy = Instantiate(enemyToSpawn);
        spawnedEnemy.transform.position = spawnPoint;

        spawnedEnemy.GetComponent<Rigidbody2D>().AddForce((transform.position - spawnedEnemy.transform.position).normalized * 6, ForceMode2D.Impulse);
        Debug.Log(spawnedEnemy);
    }

    GameObject SpawnedEnemyCheck()
    {
        if (spawnableEnemies.Count > 0)
        {
            foreach (var enemy in spawnableEnemies)
            {
                if (Random.Range(0f, 100f) <= enemy.spawnChance)
                {
                    return enemy.enemy;
                }
            }
        }
        return spawnableEnemies[0].enemy;
    }

}
