using System.Diagnostics.CodeAnalysis;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float spawnRange;
    void Start()
    {
        SpawnParticles(GlobalValues.particlesToSpawnOnStart);
    }

    public void SpawnParticles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(GlobalValues.RedParticle, new Vector2(Random.Range(-spawnRange,spawnRange), Random.Range(-spawnRange,spawnRange)), transform.rotation);
        }
    }
}
