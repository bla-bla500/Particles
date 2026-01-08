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
        TempParticleAmount = (int[])GlobalValues.particlesToSpawnOnStart.Clone();

        for (int i = 0; i < GlobalValues.particlesToSpawnOnStart.Length; i++)
        {
            GameObject.Find(Convert.ToString(i + 1) + " InputField").GetComponent<TMP_InputField>().text = Convert.ToString(GlobalValues.particlesToSpawnOnStart[i]);
        }

        SettingsExit();
    }
    public void SettingsClicked()
    {
        Time.timeScale = 0;
        settingsPanel.SetActive(true);
    }
    public void SettingsExit()
    {
        Time.timeScale = 1;
        settingsPanel.SetActive(false);
    }
    public void ApplySettings()
    {
        bool notSame = false;
        bool allTheSame = false;
        for (int i = 0; i < TempParticleAmount.Length; i++)
        {
            try
            {
                TempParticleAmount[i] = Convert.ToInt32(GameObject.Find(Convert.ToString(i+1) + " InputField").GetComponent<TMP_InputField>().text);
            }
            catch
            {
                GameObject.Find(Convert.ToString(i+1) + " InputField").GetComponent<TMP_InputField>().text = "0";
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
                            AddAmount[j] =  TempParticleAmount[j] - formerParticlesToSpawnOnStart[j];
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
        }
    }
}
