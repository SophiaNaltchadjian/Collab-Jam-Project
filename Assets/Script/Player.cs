using System.Collections;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Player : MonoBehaviour
{

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

    public int health;
    public int maxHealth;
    public bool dead;
    [SerializeField] private float mercyInvincibilityTime;
    private bool mercyInvincibility;

    [SerializeField] private UnityEngine.Transform projectileSpawnpoint;
    [SerializeField] private GameObject basicProjectile;
    [SerializeField] private float basicProjectileSpeed;
    [SerializeField] private float primaryFireDelay;
    private bool primaryFireOnDelay;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing)
        {
            return;
        }
        Dash();
        Move();
        PlayerBounds();
        RotateToMousePosition();
        PrimaryFire();
    }
    private void Move()
    {
        Vector3 movement = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0);
        transform.position += movement * speed * Time.deltaTime;
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
        canDash= true;  
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!dead && !mercyInvincibility)
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

    void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log(health);

        if (health == 0) Destroy(gameObject);
        else
        {
            StartCoroutine("MercyInvincibility");
        }
    }

    IEnumerator MercyInvincibility()
    {
        mercyInvincibility = true;

        yield return new WaitForSeconds(mercyInvincibilityTime);

        mercyInvincibility = false;
    }

    void PrimaryFire()
    {
        if (Input.GetMouseButton(0) && !primaryFireOnDelay)
        {
            var shotProjectile = Instantiate(basicProjectile);
            shotProjectile.transform.position = projectileSpawnpoint.position;
            shotProjectile.transform.rotation = projectileSpawnpoint.rotation;
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

}
