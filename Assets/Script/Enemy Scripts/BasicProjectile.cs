using UnityEngine;

public class BasicProjectile : MonoBehaviour
{
    float xBounds = 8;
    float yBounds = 6;

    public int damage;
    public bool destroyOnContact;


    // Update is called once per frame
    void Update()
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
}
