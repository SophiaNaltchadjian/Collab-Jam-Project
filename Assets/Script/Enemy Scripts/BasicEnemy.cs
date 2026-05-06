using System.Collections;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    private Player player;


    [Header("Health")]
    public int health;
    public int maxHealth;

    [Header("Projectiles & Firing")]
    public int contactDamage;
    public float fireDelay;
    public GameObject projectile;
    public float projectileSpeed;
    public Transform projectileSpawnpoint;

    [Header("Movement")]
    private float playerTrackRot;
    [SerializeField] private float maxDistanceFromPlayer;
    [SerializeField] private float moveSpeed;

    [Header("Damage & Death")]
    [SerializeField] private SpriteRenderer enemySprite;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color damageColor;
    [SerializeField] private ParticleSystem deathParticles;
    void Start()
    {
        health = maxHealth;
        player = FindAnyObjectByType<Player>();

        InvokeRepeating("ShootProjectile", fireDelay * 1.5f, fireDelay);
    }

    private void Update()
    {
        RotateToFacePlayer();
        PlayerDistanceCheck();
    }

    void RotateToFacePlayer()
    {
        if (player == null) return;

        Vector3 diff = player.gameObject.transform.position - transform.position;
        diff.Normalize();
        playerTrackRot = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, playerTrackRot - 90);
    }

    void PlayerDistanceCheck()
    {
        if (player == null) return;

        if (Vector2.Distance(gameObject.transform.position, player.gameObject.transform.position) > maxDistanceFromPlayer)
        {
            transform.position = Vector2.MoveTowards(gameObject.transform.position, player.gameObject.transform.position, moveSpeed);
        }
    }

    void ShootProjectile()
    {
        if (player == null) return;

        var shotProjectile = Instantiate(projectile);
        shotProjectile.transform.position = projectileSpawnpoint.position;
        shotProjectile.transform.rotation = projectileSpawnpoint.rotation;
        shotProjectile.GetComponent<Rigidbody2D>().AddForce(transform.up * projectileSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

            if (collision.gameObject.CompareTag("PlayerProjectile"))
            {
                var projectileScript = collision.gameObject.GetComponent<BasicProjectile>();
                TakeDamage(projectileScript.damage);
                if (projectileScript.destroyOnContact) Destroy(collision.gameObject);
            }

        
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        if (health == 0)
        {
            ParticleCheck();
            Destroy(gameObject);
        }
        else StartCoroutine(DamageStrobeEffect());
    }
    IEnumerator DamageStrobeEffect()
    {
        enemySprite.color = damageColor;

        yield return new WaitForSeconds(0.1f);

        enemySprite.color = defaultColor;
    }
    void ParticleCheck()
    {
        if (deathParticles != null)
        {
            deathParticles.transform.parent = null;
            deathParticles.Play();

            Destroy(deathParticles.gameObject, deathParticles.main.duration);
        }
    }

}
