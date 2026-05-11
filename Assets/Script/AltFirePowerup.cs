using UnityEngine;

public class AltFirePowerup : MonoBehaviour
{
    public AltFireType altfire;

    float yBounds = 8;

    void Update()
    {
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
