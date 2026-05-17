using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Scripting.APIUpdating;
using static UnityEngine.RuleTile.TilingRuleOutput;

public enum AltFireType { None, Explosive, Laser, Blast }

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
    float xBounds = 7.85f;
    float yBounds = 4.6f;
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
    public UnityEngine.Transform projectileSpawnpoint;
    [SerializeField] private GameObject basicProjectile;
    [SerializeField] private GameObject[] altFireProjectiles;
    [SerializeField] private float primaryFireDelay;
    private bool primaryFireOnDelay;
    [SerializeField] private float altFireDelay;
    private bool altFireOnDelay;

    [Header("Damage & Death")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color damageColor;
    [SerializeField] private ParticleSystem deathParticles;
    [SerializeField] private GameController gameController;
    private int ItensCount;

    [Header("Powerups")]
    bool shielded;
    float damageMod = 1;
    float speedMod = 1;
    float shieldDuration;
    float damageUpDuration;
    float speedUpDuration;
    [SerializeField] private GameObject item;
    public GameObject[] itemPositions;

    [Header("AltFire")]
    public AltFireType currAltFire = AltFireType.None;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(SpawnItens());
        //AudioMaster.AM.Sound(11);
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        anim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (ItensCount==3)
        {
            ItensCount = 0;
            health = maxHealth;
            Debug.Log("Win");
            gameController.ShowWinScreen();
            if (AudioMaster.AM)
            {
                AudioMaster.AM.StopMusic();
                AudioMaster.AM.Sound(12);
            }
        }
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
        AltFire();
    }
    private void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0).normalized; 
        transform.position += (movement * speed * Time.deltaTime) * speedMod;
    }
    private void Dash()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
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
                if (AudioMaster.AM)
                {
                    AudioMaster.AM.Sound(11);
                }
                isOnAirplaneMode = true;
                anim.SetTrigger("TurnIntoAirplane");
                playerSprite.sprite = airplaneSprite;
            }
            else
            {
                if (AudioMaster.AM)
                {
                    AudioMaster.AM.Sound(11);
                }
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
                    projectileScript.ProjectileImpact(gameObject);
                }
            }
            if (collision.gameObject.CompareTag("Powerup"))
            {
                PowerupCollect(collision.gameObject.GetComponent<PowerupFunction>());
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.CompareTag("AltFirePowerup"))
            {
                if (AudioMaster.AM)
                {
                    AudioMaster.AM.Sound(2);
                }
                AltFireChange(collision.GetComponent<AltFirePowerup>().altfire);
                Destroy(collision.gameObject);
            }
            if (collision.gameObject.CompareTag("Collectable"))
            {
                ItensCount++;
                if (AudioMaster.AM)
                {
                    AudioMaster.AM.Sound(9);
                }
                Debug.Log("Collected");
                Destroy(collision.gameObject);
            }
        }
    }
    IEnumerator SpawnItens()
    {
        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(15f);
            int posNumber = Random.Range(0, itemPositions.Length);
            Instantiate(item, itemPositions[posNumber].transform.position, Quaternion.identity);
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

    public void TakeDamage(int damage)
    {
        if (shielded)
        {
            health -= (int)(damage / 2);
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Sound(13);
            }
        }
        else
        {
            health -= damage;
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Sound(14);
            }
        }

        health = Mathf.Clamp(health, 0, maxHealth);

        uiHandler.UpdateHealth();

        if (health == 0)
        {
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Death();
                AudioMaster.AM.Sound(6);
            }
            gameController.ShowGameOver();
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

    #region "Firing"

    void PrimaryFire()
    {
        if (Input.GetMouseButton(0) && !primaryFireOnDelay)
        {
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Sound(3);
            }
            var shotProjectile = Instantiate(basicProjectile);
            shotProjectile.transform.position = projectileSpawnpoint.position;
            shotProjectile.transform.rotation = projectileSpawnpoint.rotation;
            shotProjectile.GetComponent<BasicProjectile>().damage = (int)(shotProjectile.GetComponent<BasicProjectile>().damage * damageMod);

            StartCoroutine("FireDelay");
        }
    }
    void AltFire()
    {
        if (Input.GetMouseButton(1) && !altFireOnDelay)
        {
            int altProjectileNum = -1;

            if (currAltFire == AltFireType.Explosive) altProjectileNum = 0;
            else if (currAltFire == AltFireType.Laser) altProjectileNum = 1;
            else if (currAltFire == AltFireType.Blast) altProjectileNum = 2;
            else return;

            if (AudioMaster.AM)
            {
                if (altProjectileNum == 2)
                {
                    AudioMaster.AM.Sound(5);
                }
                else
                {
                    AudioMaster.AM.Sound(4);
                }
            }
            var shotProjectile = Instantiate(altFireProjectiles[altProjectileNum]);
            shotProjectile.transform.position = projectileSpawnpoint.position;
            shotProjectile.transform.rotation = projectileSpawnpoint.rotation;
            if (shotProjectile.GetComponent<BasicProjectile>()) shotProjectile.GetComponent<BasicProjectile>().damage = (int)(shotProjectile.GetComponent<BasicProjectile>().damage * damageMod);
            else if (shotProjectile.GetComponent<AreaOfEffect>()) shotProjectile.GetComponent<AreaOfEffect>().damage = (int)(shotProjectile.GetComponent<AreaOfEffect>().damage * damageMod);
            else if (shotProjectile.GetComponent<ExplodingProjectile>()) shotProjectile.GetComponent<ExplodingProjectile>().explosionDamage = (int)(shotProjectile.GetComponent<ExplodingProjectile>().explosionDamage * damageMod);

            StartCoroutine("AltFireDelay");
        }
    }

    IEnumerator FireDelay()
    {
        primaryFireOnDelay = true;

        yield return new WaitForSeconds(primaryFireDelay);

        primaryFireOnDelay = false;
    }

    IEnumerator AltFireDelay()
    {
        altFireOnDelay = true;

        yield return new WaitForSeconds(altFireDelay);

        altFireOnDelay = false;
    }

    public void AltFireChange(AltFireType type)
    {
        currAltFire = type;
        if (currAltFire == AltFireType.None) altFireDelay = 0;
        if (currAltFire == AltFireType.Explosive) altFireDelay = 5;
        if (currAltFire == AltFireType.Laser) altFireDelay = 6;
        if (currAltFire ==AltFireType.Blast) altFireDelay = 4;
    }

    #endregion

    void PowerupCollect(PowerupFunction powerup)
    {
        if (powerup.powerupType == PowerupFunction.PowerupType.HealthUp)
        {
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Sound(7);
            }
            health += 25;
            health = Mathf.Clamp(health, 0, maxHealth);
            uiHandler.UpdateHealth();
        }
        else if (powerup.powerupType == PowerupFunction.PowerupType.Shield)
        {
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Sound(9);
            }
            shielded = true;
            shieldDuration = 10f;
        }
        else if (powerup.powerupType == PowerupFunction.PowerupType.DamageUp)
        {
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Sound(8);
            }
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
            if (AudioMaster.AM)
            {
                AudioMaster.AM.Sound(10);
            }
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
