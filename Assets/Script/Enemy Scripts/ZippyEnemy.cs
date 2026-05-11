using UnityEngine;

public class ZippyEnemy : EnemyBase
{
    bool zipping;
    float xBounds = 7.75f;
    float yBounds = 4.5f;
    void Start()
    {
            health = maxHealth;
            player = FindAnyObjectByType<Player>();

            Invoke("StartZipping", 1.5f);
    }

    void StartZipping()
    {
        zipping = true;
        InvokeRepeating("ShootAndMove", 0.5f, fireDelay);
    }

    private void Update()
    {
        RotateToFacePlayer();
        if (zipping) EnemyBounds();
    }
    void EnemyBounds()
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

    void ShootAndMove()
    {
        if (player == null) return;

        var shotProjectile = Instantiate(projectile);
        shotProjectile.transform.position = projectileSpawnpoint.position;
        shotProjectile.transform.rotation = projectileSpawnpoint.rotation;

        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, Random.Range(0f, 360f));

        gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * moveSpeed, ForceMode2D.Impulse);

    }

}
