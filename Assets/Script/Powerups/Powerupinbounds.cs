using UnityEngine;

public class Powerupinbounds : MonoBehaviour
{

    [SerializeField] private float negativeXBounds;
    [SerializeField] private float negativeYBounds;
    [SerializeField] private float positiveXBounds;
    [SerializeField] private float positiveYBounds;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.x < negativeXBounds)
        {
            transform.position = new Vector3(transform.position.x + 0.07f, transform.position.y, transform.position.z);
        }
        if (transform.position.x > positiveXBounds)
        {
            transform.position = new Vector3(transform.position.x - 0.07f, transform.position.y, transform.position.z);
        }
        if (transform.position.y < negativeYBounds)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 0.07f, transform.position.z);
        }
        if (transform.position.y > positiveYBounds)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.07f, transform.position.z);
        }
        if (transform.position.y > 1 && transform.position.x > 4.3)
        {
            transform.position = new Vector3(transform.position.x - 0.07f, transform.position.y - 0.07f, transform.position.z);
        }
    }
}
