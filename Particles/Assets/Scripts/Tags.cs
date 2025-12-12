using UnityEngine;
using System.Collections.Generic;

public class Tags : MonoBehaviour
{
    //only one bool should be active per particle
    public bool Red_p;
    public bool Yellow_p;
    public int DONTTOUCHthisThingsType;

    private void Start()
    {
        if (Red_p)
        {
            DONTTOUCHthisThingsType = 1;
        }

        if (Yellow_p)
        {
            DONTTOUCHthisThingsType = 2;
        }
    }
}
