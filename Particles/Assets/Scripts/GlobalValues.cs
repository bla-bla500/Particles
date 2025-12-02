using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    static public float globalSpeedEditable;
    static public int particlesToSpawnOnStart;
    static public float globalSpeed;
    static public List<GameObject> allParticlesOnScreen;
    public static float3[] computeShaderResults;

    [SerializeField] private GameObject temp;
    static public GameObject RedParticle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        globalSpeedEditable = 0.1f;
        particlesToSpawnOnStart = 2;
        computeShaderResults = new float3[particlesToSpawnOnStart];
        RedParticle = temp;
    }
}
