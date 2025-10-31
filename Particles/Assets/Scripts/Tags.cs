using UnityEngine;
using System.Collections.Generic;

public class Tags : MonoBehaviour
{
    public bool Particle;
    public bool Red_p;
    public List<string> tags = new List<string>();

    private void Start()
    {
        if (Particle)
        {
            tags.Add("Particle");
        }

        if (Red_p)
        {
            tags.Add("Red_p");
        }
    }
}
