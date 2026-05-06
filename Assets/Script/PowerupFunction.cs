using UnityEngine;

public class PowerupFunction : MonoBehaviour
{
    public enum PowerupType { HealthUp, Shield, DamageUp, SpeedUp }

    public PowerupType powerupType = new PowerupType();
}
