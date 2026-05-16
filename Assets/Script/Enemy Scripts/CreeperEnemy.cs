using System.Threading;
using UnityEngine;

public class CreeperEnemy : EnemyBase
{
    [HideInInspector] public bool multiplied;
    [SerializeField] GameObject enemy;
    [SerializeField] Transform spawnPos1, spawnPos2;

    void Start()
    {
        health = maxHealth;
        player = FindAnyObjectByType<Player>();
        InvokeRepeating("ShootProjectile", fireDelay, fireDelay);
    }

    // Update is called once per frame
    void Update()
    {
        RotateToFacePlayer();
        PlayerDistanceCheck();
    }
    void MultiplyInDeath()
    { 
            Vector3 scale = gameObject.transform.localScale;
            scale *= 0.5f;
            
           GameObject enemy1 = Instantiate(enemy, spawnPos1.transform.position,Quaternion.identity);
           GameObject enemy2 = Instantiate(enemy, spawnPos2.transform.position, Quaternion.identity);
            enemy1.transform.localScale = scale;
            enemy2.transform.localScale = scale;
            enemy1.GetComponent<CreeperEnemy>().maxHealth /= 2;
            enemy2.GetComponent<CreeperEnemy>().maxHealth /= 2;
            enemy1.GetComponent<CreeperEnemy>().multiplied = true;
            enemy2.GetComponent<CreeperEnemy>().multiplied = true;
    }
    override public void Death()
    {
        if (!multiplied)
        {
            MultiplyInDeath();
            PowerupCheck();
        }
        ParticleCheck();
        Destroy(gameObject, 0.1f);
    }

}
