using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour
{
    [SerializeField] private UIHandler uiHandler;

    [Header("Movement")]
    public float speed;
    private Rigidbody2D rb;
    public float dashSpeed = 20f;
    public float dashDuration = 0.1f;
    public float dashCooldown = 0.1f;
    bool isDashing;
    bool canDash = true;
    TrailRenderer trailRenderer;
    float xBounds = 6.2f;
    float yBounds = 4.5f;
    private Animator anim;

    [Header("Switch modes")]
    private bool isOnAirplaneMode=false;
    public Sprite airplaneSprite;
    public Sprite emailSprite;

    [Header("Health & Invincibility")]
    public int health = 100;
    public int maxHealth = 100;
    public bool dead;
    [SerializeField] private float mercyInvincibilityTime;
    private bool mercyInvincibility;

    [Header("Projectiles & Firing")]
    [SerializeField] private UnityEngine.Transform projectileSpawnpoint;
    [SerializeField] private GameObject basicProjectile;
    [SerializeField] private float basicProjectileSpeed;
    [SerializeField] private float primaryFireDelay;
    private bool primaryFireOnDelay;

    [Header("Damage & Death")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color damageColor;
    [SerializeField] private ParticleSystem deathParticles;

    [Header("Powerups")]
    bool shielded;
    float damageMod = 1;
    float speedMod = 1;
    float shieldDuration;
    float damageUpDuration;
    float speedUpDuration;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        anim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        PowerupDurationCheck();
        PlayerBounds();

        if (isDashing)
        {
            return;
        }
        Dash();
        Move();
        RotateToMousePosition();
        TurnIntoAirplane();
        if (!isOnAirplaneMode)
        {
            return;
        }
        PrimaryFire();
    }
    private void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0); 
        transform.position += (movement * speed * Time.deltaTime) * speedMod;
    }
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(DashCoroutine());
        }
    }
    IEnumerator DashCoroutine()
    {
        canDash = false;
        isDashing = true;

        trailRenderer.emitting = true;

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 direction = new Vector2(x, y);
        rb.linearVelocity = direction.normalized * dashSpeed; //dash movement
        yield return new WaitForSeconds(dashDuration);
        rb.linearVelocity = new Vector2(0f,0f);
        isDashing = false;
        trailRenderer.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;  
    }
    private void TurnIntoAirplane()
    {
        if (Input.GetKeyDown(KeyCode.E) )
        {
            if (isOnAirplaneMode==false)
            {
                isOnAirplaneMode = true;
                anim.SetTrigger("TurnIntoAirplane");
                playerSprite.sprite = airplaneSprite;
            }
            else
            {
                isOnAirplaneMode = false;
                anim.SetTrigger("TurnIntoEmail");
                playerSprite.sprite = emailSprite;
            }
        }
       
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!dead)
        {
            if (!mercyInvincibility)
            {
                if (collision.gameObject.CompareTag("Enemy"))
                {
                    TakeDamage(collision.gameObject.GetComponent<BasicEnemy>().contactDamage);
                }
                if (collision.gameObject.CompareTag("EnemyProjectile"))
                {
                    var projectileScript = collision.gameObject.GetComponent<BasicProjectile>();
                    TakeDamage(projectileScript.damage);
                    if (projectileScript.destroyOnContact) Destroy(collision.gameObject);
                }
            }
            if (collision.gameObject.CompareTag("Powerup"))
            {
                PowerupCollect(collision.gameObject.GetComponent<PowerupFunction>());
                Destroy(collision.gameObject);
            }
        }
    }

    void PlayerBounds()
    {
        if (transform.position.x > xBounds)
        {
            transform.position = new Vector3(xBounds, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -xBounds)
        {
            transform.position = new Vector3(-xBounds, transform.position.y, transform.position.z);
        }

        if (transform.position.y > yBounds)
        {
            transform.position = new Vector3(transform.position.x, yBounds, transform.position.z);
        }
        if (transform.position.y < -yBounds)
        {
            transform.position = new Vector3(transform.position.x, -yBounds, transform.position.z);
        }
    }

    void RotateToMousePosition()
    {
        Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
    }

    #region "Damage & Death"

    void TakeDamage(int damage)
    {
        if (shielded) health -= (int)(damage / 2);
        else health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        uiHandler.UpdateHealth();

        if (health == 0)
        {
            ParticleCheck();
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(MercyInvincibility());
            StartCoroutine(DamageStrobeEffect());
        }
    }

    IEnumerator MercyInvincibility()
    {
        mercyInvincibility = true;

        yield return new WaitForSeconds(mercyInvincibilityTime);

        mercyInvincibility = false;
    }
    IEnumerator DamageStrobeEffect()
    {
        playerSprite.color = damageColor;

        yield return new WaitForSeconds(0.1f);

        playerSprite.color = defaultColor;

        yield return new WaitForSeconds(0.1f);

        if (mercyInvincibility) StartCoroutine(DamageStrobeEffect());
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

    #endregion

    void PrimaryFire()
    {
        if (Input.GetMouseButton(0) && !primaryFireOnDelay)
        {
            var shotProjectile = Instantiate(basicProjectile);
            shotProjectile.transform.position = projectileSpawnpoint.position;
            shotProjectile.transform.rotation = projectileSpawnpoint.rotation;
            shotProjectile.GetComponent<BasicProjectile>().damage = (int)(shotProjectile.GetComponent<BasicProjectile>().damage * damageMod);
            shotProjectile.GetComponent<Rigidbody2D>().AddForce(transform.up * basicProjectileSpeed, ForceMode2D.Impulse);

            StartCoroutine("FireDelay");
        }
    }
    IEnumerator FireDelay()
    {
        primaryFireOnDelay = true;

        yield return new WaitForSeconds(primaryFireDelay);

        primaryFireOnDelay = false;
    }

    void PowerupCollect(PowerupFunction powerup)
    {
        if (powerup.powerupType == PowerupFunction.PowerupType.HealthUp)
        {
            health += 25;
            health = Mathf.Clamp(health, 0, maxHealth);
            uiHandler.UpdateHealth();
        }
        else if (powerup.powerupType == PowerupFunction.PowerupType.Shield)
        {
            shielded = true;
            shieldDuration = 10f;
        }
        else if (powerup.powerupType == PowerupFunction.PowerupType.DamageUp)
        {
            if (damageUpDuration > 0)
            {
                damageUpDuration = 10f;
                return;
            }

            damageMod += 0.5f;
            damageUpDuration = 10f;
        }
        else if (powerup.powerupType == PowerupFunction.PowerupType.SpeedUp)
        {
            if (speedUpDuration > 0)
            {
                speedUpDuration = 10f;
                return;
            }

            speedMod += 0.5f;
            speedUpDuration = 10f;
        }
    }

    void PowerupDurationCheck()
    {
        if (shielded)
        {
            shieldDuration -= Time.deltaTime;
            shieldDuration = Mathf.Clamp(shieldDuration, 0, 30);
            if (shieldDuration == 0) shielded=false;
        }
        if (damageUpDuration > 0)
        {
            damageUpDuration -= Time.deltaTime;
            damageUpDuration = Mathf.Clamp(damageUpDuration, 0, 30);
            if (damageUpDuration == 0) damageMod -= 0.5f;
        }
        if (speedUpDuration > 0)
        {
            speedUpDuration -= Time.deltaTime;
            speedUpDuration = Mathf.Clamp(speedUpDuration, 0, 30);
            if (speedUpDuration == 0) speedMod -= 0.5f;
        }
    }

}
