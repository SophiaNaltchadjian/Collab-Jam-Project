using UnityEngine;

[System.Serializable]
public class DroppedPowerup
{
    public GameObject powerup;
    [Range(0.0f, 100.0f)] public float dropChance;

}
