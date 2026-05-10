using UnityEngine;

public class PowerupFunction : MonoBehaviour
{
    public enum PowerupType { HealthUp, Shield, DamageUp, SpeedUp }

    public PowerupType powerupType = new PowerupType();

    private bool fading;
    private float fadingValue = 1;
    [SerializeField] private SpriteRenderer sprite;

    public void Start()
    {
        Invoke("ToggleFade", 5f);
    }

    void ToggleFade()
    {
        fading = true;
    }

    private void Update()
    {
        if (fading)
        {
            fadingValue -= Time.deltaTime / 5;
            fadingValue = Mathf.Clamp (fadingValue, 0, 10);
            sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, fadingValue);
            if (fadingValue <= 0) Destroy(gameObject);
        }
    }

}
