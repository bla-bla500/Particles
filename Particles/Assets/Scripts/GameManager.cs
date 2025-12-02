using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float spawnRange;

    public ComputeShader computeShader;
    private ComputeBuffer computeBuffer;
    private ComputeBuffer debugBuffer;
    private ComputeBuffer staticComputeBuffer;
    private Vector3[] allParticlesOnScreenPositions;

    private void Awake()
    {
        GlobalValues.allParticlesOnScreen = AllParticlesOnScreen();
        for (int i = 0; i < GlobalValues.allParticlesOnScreen.Count; i++)
        {
            allParticlesOnScreenPositions[i] = GlobalValues.allParticlesOnScreen[i].transform.position;
        }
    }
    private void Start()
    {
        SpawnParticles(GlobalValues.particlesToSpawnOnStart);
    }
    
    void Update()
    {

        Vector3[] tempForBuffer = new Vector3[GlobalValues.allParticlesOnScreen.Count];
        for (int i = 0; i < GlobalValues.allParticlesOnScreen.Count; i++)
        {
            tempForBuffer[i] = GlobalValues.allParticlesOnScreen[i].transform.position;
        }
        if (tempForBuffer.Length > 0) 
        {
            staticComputeBuffer = new ComputeBuffer(tempForBuffer.Length, 12);
            computeBuffer = new ComputeBuffer(tempForBuffer.Length, 12);
            debugBuffer = new ComputeBuffer(tempForBuffer.Length, 12);
            staticComputeBuffer.SetData(tempForBuffer);
            for (int i = 0; i < tempForBuffer.Length; i++)
            {
                tempForBuffer[i] = new Vector3(0,0,0);
            }
            computeBuffer.SetData(tempForBuffer);
            debugBuffer.SetData(tempForBuffer);


            computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "staticBuffer", staticComputeBuffer);
            computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "theBuffer", computeBuffer);
            computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "debugBuffer", debugBuffer);

            int howManyThreadGroups = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(GlobalValues.allParticlesOnScreen.Count)/64));

            computeShader.Dispatch(computeShader.FindKernel("CSMain"), howManyThreadGroups, 1, 1);

            GlobalValues.computeShaderResults = new float3[computeBuffer.count];
            computeBuffer.GetData(GlobalValues.computeShaderResults);
            
            
            float3[] debugResults = new float3[debugBuffer.count];
            debugBuffer.GetData(debugResults);
            //for (int i = 0; i < debugResults.Length; i++)
            //{
            //    Debug.Log(i + ": " + debugResults[i]);
            //}
            

            staticComputeBuffer.Release();
            computeBuffer.Release();
            debugBuffer.Release();
        }
    }
    

    public void SpawnParticles(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(GlobalValues.RedParticle, new Vector2(UnityEngine.Random.Range(-spawnRange,spawnRange), UnityEngine.Random.Range(-spawnRange,spawnRange)), transform.rotation);
        }
    }

    public List<GameObject> AllParticlesOnScreen()
    {
        List<GameObject> list = new List<GameObject>();
        GameObject[] array = GameObject.FindGameObjectsWithTag("Search");
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].GetComponent<Tags>().tags.Contains("Particle"))
            {
                list.Add(array[i]);
            }
        }
        list.TrimExcess();
        return list;
    }
}
