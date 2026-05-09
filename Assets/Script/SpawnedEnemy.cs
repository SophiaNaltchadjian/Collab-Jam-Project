using UnityEngine;

[System.Serializable]
public class SpawnedEnemy
{
    public GameObject enemy;
    [Range(0f, 100f)] public float spawnChance;
}
