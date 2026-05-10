using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    float xBounds = 8;
    float yBounds = 6;

    public int damage;
    public float speed;
    public bool destroyOnContact;


    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
    }
    void Update()
    {
        BoundsCheck();
    }

    public void BoundsCheck()
    {
        if (transform.position.x > xBounds)
        {
            Destroy(gameObject);
        }
        if (transform.position.x < -xBounds)
        {
            Destroy(gameObject);
        }

        if (transform.position.y > yBounds)
        {
            Destroy(gameObject);
        }
        if (transform.position.y < -yBounds)
        {
            Destroy(gameObject);
        }
    }

    virtual public void ProjectileImpact(GameObject impactObject)
    {
        if (impactObject.GetComponent<EnemyBase>()) impactObject.GetComponent<EnemyBase>().TakeDamage(damage);
        if (impactObject.GetComponent<Player>()) impactObject.GetComponent<Player>().TakeDamage(damage);
        if (destroyOnContact) Destroy(gameObject);
    }

}
