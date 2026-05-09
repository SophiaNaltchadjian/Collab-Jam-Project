using UnityEngine;

public class Pulse : MonoBehaviour
{
    public float speed = 2f;
    public float amount = 0.1f;

    Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    void Update()
    {
        float scale = 1 + Mathf.Sin(Time.time * speed) * amount;
        transform.localScale = startScale * scale;
    }
    
    
}
