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
    private ComputeBuffer interactionMatrix;
    private ComputeBuffer debugBuffer;
    private ComputeBuffer staticComputeBuffer;
    private ComputeBuffer staticVeclocityBuffer;
    private Vector3[] allParticlesOnScreenPositions;
    private int howManyTypesOfParticles = 2;

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

        float[] tempForBuffer = new float[howManyTypesOfParticles*howManyTypesOfParticles];
        interactionMatrix = new ComputeBuffer(tempForBuffer.Length, 4);
        for (int i = 0; i < tempForBuffer.Length; i++)
        {
            tempForBuffer[i] = GetRandomStength();
        }

        //display matrix values
        for (int i = 0; i < tempForBuffer.Length; i++)
        {
            Debug.Log(tempForBuffer[i]);
        }

        interactionMatrix.SetData(tempForBuffer);
        computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "interactionMatrix", interactionMatrix);

    }
    
    void Update()
    {

        Vector3[] tempForBuffer = new Vector3[GlobalValues.allParticlesOnScreen.Count];
        for (int i = 0; i < GlobalValues.allParticlesOnScreen.Count; i++)
        {
            tempForBuffer[i] = GlobalValues.allParticlesOnScreen[i].transform.position;
            tempForBuffer[i].z = GlobalValues.allParticlesOnScreen[i].GetComponent<Tags>().DONTTOUCHthisThingsType;
        }
        if (tempForBuffer.Length > 0) 
        {
            staticComputeBuffer = new ComputeBuffer(tempForBuffer.Length, 12);
            staticVeclocityBuffer = new ComputeBuffer (tempForBuffer.Length, 8);
            computeBuffer = new ComputeBuffer(tempForBuffer.Length, 12);
            debugBuffer = new ComputeBuffer(tempForBuffer.Length, 12);

            staticComputeBuffer.SetData(tempForBuffer);

            Vector2[] tempVelocityForBuffer = new Vector2[GlobalValues.allParticlesOnScreen.Count];

            for (int i = 0; i < GlobalValues.allParticlesOnScreen.Count; i++)
            {
                tempVelocityForBuffer[i] = GlobalValues.allParticlesOnScreen[i].GetComponent<Rigidbody2D>().linearVelocity;
            }
            staticVeclocityBuffer.SetData(tempVelocityForBuffer);

            for (int i = 0; i < tempForBuffer.Length; i++)
            {
                tempForBuffer[i] = new Vector3(0,0,0);
            }
            computeBuffer.SetData(tempForBuffer);
            debugBuffer.SetData(tempForBuffer);

            computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "velocityBuffer", staticVeclocityBuffer);
            computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "staticBuffer", staticComputeBuffer);
            computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "theBuffer", computeBuffer);
            computeShader.SetBuffer(computeShader.FindKernel("CSMain"), "debugBuffer", debugBuffer);

            int howManyThreadGroups = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(GlobalValues.allParticlesOnScreen.Count)/64));

            computeShader.Dispatch(computeShader.FindKernel("CSMain"), howManyThreadGroups, 1, 1);

            GlobalValues.computeShaderResults = new float3[computeBuffer.count];
            computeBuffer.GetData(GlobalValues.computeShaderResults);
            
            
            float3[] debugResults = new float3[debugBuffer.count];
            debugBuffer.GetData(debugResults);
            //Debug.Log(debugResults[1]);
            /*for (int i = 0; i < debugResults.Length; i++)
            {
                Debug.Log(i + ": " + debugResults[i]);
            }*/

            staticVeclocityBuffer.Release();
            staticComputeBuffer.Release();
            computeBuffer.Release();
            debugBuffer.Release();
        }
    }
    

    public void SpawnParticles(int[] amount)
    {
        GameObject particlePrefab = GlobalValues.RedParticle;
        for (int i = 0; i < amount.Length; i++)
        {
            switch (i)
            {
                case 0:
                    particlePrefab = GlobalValues.RedParticle;
                    break;
                case 1:
                    particlePrefab = GlobalValues.YellowParticle;
                    break;
            }
            for (int j = 0; j < amount[i]; j++)
            {
                Instantiate(particlePrefab, new Vector2(UnityEngine.Random.Range(-spawnRange, spawnRange), UnityEngine.Random.Range(-spawnRange, spawnRange)), transform.rotation);
            }
            

        }
    }

    public List<GameObject> AllParticlesOnScreen()
    {
        List<GameObject> list = new List<GameObject>();
        GameObject[] array = GameObject.FindGameObjectsWithTag("Search");
        for (int i = 0; i < array.Length; i++)
        {
            list.Add(array[i]);
        }
        list.TrimExcess();
        return list;
    }

    private float GetRandomStength()
    {
        float maybe = 0;
        while (-0.11 < maybe && maybe < 0.11)
        {
            maybe = (float)Math.Round(Convert.ToDouble(UnityEngine.Random.Range(-100, 101)) / 100, 2);
        }
        return maybe;
    }
}
