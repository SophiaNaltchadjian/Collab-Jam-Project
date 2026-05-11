using UnityEngine;

public class ExplodingProjectile : BasicProjectile
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private float autoExplosionTime;
    public int explosionDamage;

    private void Start()
    {
        GetComponent<Rigidbody2D>().AddForce(transform.up * speed, ForceMode2D.Impulse);
        Invoke("Explode", autoExplosionTime);
    }
    override public void ProjectileImpact(GameObject impactObject)
    {
        if (impactObject.GetComponent<EnemyBase>()) impactObject.GetComponent<EnemyBase>().TakeDamage(damage);
        if (impactObject.GetComponent<Player>()) impactObject.GetComponent<Player>().TakeDamage(damage);
        Explode();
    }

    void Explode()
    {
        var explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.transform.rotation = transform.rotation;
        explosion.GetComponent<AreaOfEffect>().damage = explosionDamage;

        Destroy(gameObject);
    }

}
