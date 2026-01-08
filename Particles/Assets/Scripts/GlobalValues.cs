using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class GlobalValues : MonoBehaviour
{
    [SerializeField] private float globalSpeedEditable;
    static public int[] particlesToSpawnOnStart;
    static public float globalSpeed;
    static public List<GameObject> allParticlesOnScreen;
    public static float3[] computeShaderResults;
    [SerializeField] private GameObject RedParticleEditable;
    [SerializeField] private GameObject YellowParticleEditable;
    static public GameObject RedParticle;
    static public GameObject YellowParticle;
    [SerializeField]private float dampingMultiplierEditable;
    static public float dampingMultiplier;

   private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        particlesToSpawnOnStart = new int[] {500, 500};
        globalSpeed = globalSpeedEditable / 1000;
        computeShaderResults = new float3[particlesToSpawnOnStart.Sum()];
        RedParticle = RedParticleEditable;
        YellowParticle = YellowParticleEditable;
        dampingMultiplier = dampingMultiplierEditable;
    }

    public void RefreshTimeScale()
    {
        globalSpeed = globalSpeedEditable / 1000;
    }
}
