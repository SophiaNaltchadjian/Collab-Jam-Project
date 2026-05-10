using System.Collections;
using UnityEngine;

public class WaveShooterEnemy : EnemyBase
{
    public Transform[] waveProjectileTransforms;
    void Start()
    {
        health = maxHealth;
        player = FindAnyObjectByType<Player>();

        if (fires) InvokeRepeating("ShootProjectile", fireDelay / 1.5f, fireDelay);
    }
    override public void ShootProjectile()
    {
        if (player != null) StartCoroutine("ProjectileWave");
    }

    IEnumerator ProjectileWave()
    {

        for (int t = 0; t < 3; t++)
        {
            for (int i = 0; i < waveProjectileTransforms.Length; i++)
            {

                var shotProjectile = Instantiate(projectile);
                shotProjectile.transform.position = waveProjectileTransforms[i].position;
                shotProjectile.transform.rotation = waveProjectileTransforms[i].rotation;
            }
            yield return new WaitForSeconds(0.25f);
        }
    }

}
