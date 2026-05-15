using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Player player;

    [Header("Health")]
    public int health;
    public int maxHealth;

    [Header("Projectiles & Firing")]
    public bool fires;
    public int contactDamage;
    public float fireDelay;
    public GameObject projectile;
    public Transform projectileSpawnpoint;

    [Header("Movement")]
    [HideInInspector] public float playerTrackRot;
    public float maxDistanceFromPlayer;
    public float moveSpeed;

    public SpriteRenderer enemySprite;
    public Color defaultColor;
    public Color damageColor;
    public ParticleSystem deathParticles;
    public List<DroppedPowerup> droppedPowerups = new();
    void Start()
    {
        health = maxHealth;
        player = FindAnyObjectByType<Player>();

        if (fires) InvokeRepeating("ShootProjectile", fireDelay, fireDelay);
    }
    private void Update()
    {
        RotateToFacePlayer();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("PlayerProjectile"))
        {
            var projectileScript = collision.gameObject.GetComponent<BasicProjectile>();
            projectileScript.ProjectileImpact(gameObject);
        }


    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0)
        {
            Death();
         
        }
        else StartCoroutine(DamageStrobeEffect());
  
    }

    IEnumerator DamageStrobeEffect()
    {
        enemySprite.color = damageColor;

        yield return new WaitForSeconds(0.1f);

        enemySprite.color = defaultColor;
    }
    public void ParticleCheck()
    {
        if (deathParticles != null)
        {
            deathParticles.transform.parent = null;
            deathParticles.Play();

            Destroy(deathParticles.gameObject, deathParticles.main.duration);
        }
    }

    virtual public void Death()
    {
        PowerupCheck();
        ParticleCheck();
        Destroy(gameObject,0.1f);
    }

    public void PowerupCheck()
    {
        if (droppedPowerups.Count > 0)
        {
            foreach (var item in droppedPowerups)
            {
                if (Random.Range(0f, 100f) <= item.dropChance)
                {
                    var droppedPowerup = Instantiate(item.powerup);
                    droppedPowerup.transform.position = transform.position;
                    return;
                }
            }
        }
    }
    public void PlayerDistanceCheck()
    {
        if (player == null) return;

        if (Vector2.Distance(gameObject.transform.position, player.gameObject.transform.position) > maxDistanceFromPlayer)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, player.gameObject.transform.position, moveSpeed);
        }
    }

    public void RotateToFacePlayer()
    {
        if (player == null) return;

        Vector3 diff = player.gameObject.transform.position - transform.position;
        diff.Normalize();
        playerTrackRot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, playerTrackRot - 90);
    }
    virtual public void ShootProjectile()
    {
        if (player == null) return;

        var shotProjectile = Instantiate(projectile);
        shotProjectile.transform.position = projectileSpawnpoint.position;
        shotProjectile.transform.rotation = projectileSpawnpoint.rotation;
    }

}
