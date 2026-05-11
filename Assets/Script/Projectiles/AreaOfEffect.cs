using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AreaOfEffect : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private SpriteRenderer sprite;
    public int damage;
    [SerializeField] private bool playerOwned;
    private float fadeTimer = 1;
    private List<GameObject> hitObjects = new();

    // Update is called once per frame
    void Update()
    {
        fadeTimer -= Time.deltaTime / duration;
        fadeTimer = Mathf.Clamp(fadeTimer, 0, 10);
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, fadeTimer);
        if (fadeTimer <= 0) Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (hitObjects.Contains(collision.gameObject)) return;

        if (collision.GetComponent<EnemyBase>() && playerOwned) collision.GetComponent<EnemyBase>().TakeDamage(damage);
        if (collision.GetComponent<Player>() && !playerOwned) collision.GetComponent<Player>().TakeDamage(damage);
        hitObjects.Add(collision.gameObject);
    }

}
