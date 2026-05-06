using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private GameObject[] enemySpawnList;

    private void Start()
    {
        InvokeRepeating("EnemySpawn", 3, 3);
    }

    void EnemySpawn()
    {
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

        var spawnedEnemy = Instantiate(enemySpawnList[Random.Range(0, enemySpawnList.Length)]);
        spawnedEnemy.transform.position = spawnPoint;

        spawnedEnemy.GetComponent<Rigidbody2D>().AddForce((transform.position - spawnedEnemy.transform.position).normalized * 6, ForceMode2D.Impulse);
    }

}
