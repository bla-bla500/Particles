using UnityEngine;

public class GlobalValues : MonoBehaviour
{
    static public float globalSpeedEditable;
    static public int particlesToSpawnOnStart;

    [SerializeField] private GameObject temp;
    static public GameObject RedParticle;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        globalSpeedEditable = 0.1f;
        particlesToSpawnOnStart = 10;
        RedParticle = temp;
    }
}
