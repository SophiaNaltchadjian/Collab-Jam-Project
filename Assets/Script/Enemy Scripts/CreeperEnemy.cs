using System.Threading;
using UnityEngine;

public class CreeperEnemy : EnemyBase
{
    private bool hasMultiplied = false;
    [SerializeField] GameObject Enemy;
    [SerializeField] Transform spawnPos;
    [SerializeField] GameController gameController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = maxHealth;
        player = FindAnyObjectByType<Player>();
        gameController = FindAnyObjectByType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        
        RotateToFacePlayer();
        MultiplyInDeath();
        fireDelay += Time.deltaTime;
        if (fireDelay > 2)
        {
            fireDelay = 0;
            ShootProjectile();
        }
    }
    void MultiplyInDeath()
    {
        if (health <=0 && hasMultiplied==false && gameController.creeperGenerations <= 1)
        {
           
            Vector3 scale = gameObject.transform.localScale;
            scale *= 0.5f;
            
            Enemy.transform.localScale = scale;
           GameObject enemy1= Instantiate(Enemy,gameObject.transform.position,Quaternion.identity);
            GameObject enemy2 = Instantiate(Enemy, spawnPos.transform.position, Quaternion.identity);
            enemy1.transform.localScale=scale;
            enemy2.transform.localScale = scale;
            enemy1.GetComponent<CreeperEnemy>().health /= 2;
            enemy2.GetComponent<CreeperEnemy>().health /= 2;
            hasMultiplied = true;
            gameController.creeperGenerations++;
         
        }
    }
   
}
