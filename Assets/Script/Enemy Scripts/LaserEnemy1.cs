using System.Collections;
using UnityEngine;

public class LaserEnemy : EnemyBase
{
    public ParticleSystem chargeParticle;
    private bool stopRotating;
    void Start()
    {
       health = maxHealth;
       player = FindAnyObjectByType<Player>();

       InvokeRepeating("ShootProjectile", fireDelay, fireDelay);
    }

    void Update()
    {
        if (!stopRotating) RotateToFacePlayer();
    }
    override public void ShootProjectile()
    {
        if (player != null) StartCoroutine("LaserCharge");
    }

    IEnumerator LaserCharge()
    {
        chargeParticle.Play();

        yield return new WaitForSeconds(2f);

        StartCoroutine("LaserFreeze");
    }

    IEnumerator LaserFreeze()
    {
        stopRotating = true;

        yield return new WaitForSeconds(0.5f);
        
        StartCoroutine("LaserFire");
    }

    IEnumerator LaserFire()
    {
        var shotProjectile = Instantiate(projectile);
        shotProjectile.transform.position = projectileSpawnpoint.position;
        shotProjectile.transform.rotation = projectileSpawnpoint.rotation;

        yield return new WaitForSeconds(0.2f);

        stopRotating = false;
    }
}
