using System;
using System.Linq;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class UIFuntionaity : MonoBehaviour
{
    GameObject settingsPanel;
    GameObject matrixPanel;
    GameManager gameManager;
    private int[] TempParticleAmount;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        settingsPanel = GameObject.Find("Settings Panel");
        matrixPanel = GameObject.Find("Matrix Panel");
        TempParticleAmount = (int[])GlobalValues.particlesToSpawnOnStart.Clone();

        for (int i = 0; i < GlobalValues.particlesToSpawnOnStart.Length; i++)
        {
            GameObject.Find(Convert.ToString(i + 1) + " InputField").GetComponent<TMP_InputField>().text = Convert.ToString(GlobalValues.particlesToSpawnOnStart[i]);
        }

        //set interaction matrix
        Vector2 matrixPoint = new Vector2(1, 1);
        float[] data = new float[GameManager.interactionMatrix.count];
        GameManager.interactionMatrix.GetData(data);
        for (int i = 0; i < GameManager.interactionMatrix.count; i++)
        {
            GameObject.Find("matrix(" + matrixPoint.x + "," + matrixPoint.y + ")").GetComponent<TMP_InputField>().text = Convert.ToString(data[i]);
            matrixPoint.x += 1;
            if (matrixPoint.x > 2)
            {
                matrixPoint.x = 1;
                matrixPoint.y += 1;
            }
        }

        settingsPanel.SetActive(false);
        matrixPanel.SetActive(false);
    }
    public void SettingsClicked(string whichPanel)
    {
        Time.timeScale = 0;
        switch (whichPanel)
        {
            case "settings":
                settingsPanel.SetActive(true);
                break;
            case "matrix":
                matrixPanel.SetActive(true);
                break;
        }
        
    }
    public void SettingsExit(string whichPanel)
    {
        switch (whichPanel)
        {
            case "settings":
                Time.timeScale = 1;
                settingsPanel.SetActive(false);
                matrixPanel.SetActive(false);
                break;
            case "matrix":
                matrixPanel.SetActive(false);
                break;
        }
    }

    public void ResetParticles()
    {
        gameManager.DeleteParticles(GlobalValues.particlesToSpawnOnStart);
        gameManager.SpawnParticles(GlobalValues.particlesToSpawnOnStart);
    }
    public void ApplySettings()
    {
        bool enableEverything = false;
        if (matrixPanel.activeSelf == false)
        {
            matrixPanel.SetActive(true);
            enableEverything = true;
        }

        //matrix
        Vector2 matrixPoint = new Vector2(1, 1);
        float[] data = new float[GameManager.interactionMatrix.count];
        float[] newData = new float[GameManager.interactionMatrix.count];
        GameManager.interactionMatrix.GetData(data);

        newData = data;
        for (int i = 0; i < data.Length; i++)
        {
            try
            {
                newData[i] = Convert.ToSingle(GameObject.Find("matrix(" + matrixPoint.x + "," + matrixPoint.y + ")").GetComponent<TMP_InputField>().text);
            }
            catch
            {
                GameObject.Find("matrix(" + matrixPoint.x + "," + matrixPoint.y + ")").GetComponent<TMP_InputField>().text = Convert.ToString(data[i]);
                Debug.Log("Invalid Entry");
            }

            matrixPoint.x += 1;
            if (matrixPoint.x > 2)
            {
                matrixPoint.x = 1;
                matrixPoint.y += 1;
            }
        }

        GameManager.interactionMatrix.SetData(newData);
        matrixPoint = new Vector2(1, 1);
        for (int i = 0; i < data.Length; i++)
        {
            GameObject.Find("matrix(" + matrixPoint.x + "," + matrixPoint.y + ")").GetComponent<TMP_InputField>().text = Convert.ToString(newData[i]);
            matrixPoint.x += 1;
            if (matrixPoint.x > 2)
            {
                matrixPoint.x = 1;
                matrixPoint.y += 1;
            }
        }


        //Particle amounts
        bool notSame = false;
        bool allTheSame = false;
        for (int i = 0; i < TempParticleAmount.Length; i++)
        {
            try
            {
                TempParticleAmount[i] = Convert.ToInt32(GameObject.Find(Convert.ToString(i + 1) + " InputField").GetComponent<TMP_InputField>().text);
            }
            catch
            {
                GameObject.Find(Convert.ToString(i + 1) + " InputField").GetComponent<TMP_InputField>().text = "0";
                TempParticleAmount[i] = Convert.ToInt32(GameObject.Find(Convert.ToString(i + 1) + " InputField").GetComponent<TMP_InputField>().text);
                Debug.Log("Invalid value");
            }

            if (TempParticleAmount[i] != GlobalValues.particlesToSpawnOnStart[i])
            {
                notSame = true;
            }
            else if (i == TempParticleAmount.Length - 1 && notSame == false)
            {
                allTheSame = true;
            }
        }

        if (allTheSame == false)
        {
            int[] DeleteAmount = new int[TempParticleAmount.Length];
            int[] AddAmount = new int[TempParticleAmount.Length];

            int[] formerParticlesToSpawnOnStart = (int[])GlobalValues.particlesToSpawnOnStart.Clone();
            GlobalValues.particlesToSpawnOnStart = (int[])TempParticleAmount.Clone();

            for (int i = 0; i < TempParticleAmount.Length; i++)
            {
                if (TempParticleAmount[i] < formerParticlesToSpawnOnStart[i])
                {
                    DeleteAmount[i] = formerParticlesToSpawnOnStart[i] - TempParticleAmount[i];
                }
                else
                {
                    DeleteAmount[i] = 0;
                    if (TempParticleAmount[i] > formerParticlesToSpawnOnStart[i])
                    {
                        for (int j = 0; j < TempParticleAmount.Length; j++)
                        {
                            AddAmount[j] = TempParticleAmount[j] - formerParticlesToSpawnOnStart[j];
                        }
                    }
                    else
                    {
                        AddAmount[i] = 0;
                    }
                }
            }
            gameManager.DeleteParticles(DeleteAmount);
            gameManager.SpawnParticles(AddAmount);

            foreach (GameObject i in GlobalValues.allParticlesOnScreen)
            {
                i.GetComponent<AttractToSelf>().FindIndex();
            }
        }
           if (enableEverything)
        {
            matrixPanel.SetActive(false);
        }
    }
}
