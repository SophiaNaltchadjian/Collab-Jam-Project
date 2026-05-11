using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    public List<SpawnedEnemy> spawnableEnemies = new();
    public List<AltFirePowerup> altFirePowerups = new();

    private Player player;

    private void Start()
    {
        player = FindAnyObjectByType<Player>();
        InvokeRepeating("EnemySpawn", 2.5f, 3f);
        InvokeRepeating("AltFireSpawn", 5, 10);
    }

    void EnemySpawn()
    {
        if (player == null) return;

        var enemyToSpawn = SpawnedEnemyCheck();
        if (enemyToSpawn == null) return;

        var spawnPointOrigin = spawnPoints[Random.Range(0, spawnPoints.Length)];

        Vector2 spawnPoint = Vector2.zero;

        if (spawnPointOrigin == spawnPoints[0] || spawnPointOrigin == spawnPoints[1])
        {
            spawnPoint = new Vector2(spawnPointOrigin.position.x + Random.Range(-7.5f, 7.5f), spawnPointOrigin.position.y);
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

    void AltFireSpawn()
    {
        if (player == null) return;

        var spawnPointOrigin = spawnPoints[Random.Range(0, 2)];

        Vector2 spawnPoint = new Vector2(spawnPointOrigin.position.x + Random.Range(-7.5f, 7.5f), spawnPointOrigin.position.y);

        var spawnedAltFire = Instantiate(altFirePowerups[Random.Range(0, altFirePowerups.Count + 1)]);
        spawnedAltFire.transform.position = spawnPoint;

        if (spawnPointOrigin == spawnPoints[0]) spawnedAltFire.GetComponent<Rigidbody2D>().AddForce(transform.up * -4, ForceMode2D.Impulse);
        else spawnedAltFire.GetComponent<Rigidbody2D>().AddForce(transform.up * 4, ForceMode2D.Impulse);
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
