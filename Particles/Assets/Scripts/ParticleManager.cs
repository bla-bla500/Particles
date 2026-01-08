using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Linq;
public class ParticleManager : MonoBehaviour
{


    public float DistancetoObject(GameObject object1, GameObject object2)
    {
        float distance = (float)Math.Sqrt(Math.Pow((object1.transform.position.x - object2.transform.position.x), 2) +
        Math.Pow((object1.transform.position.y - object2.transform.position.y), 2));
        return distance;
    }

    protected Vector2 DirectionToObject(GameObject object1, GameObject object2)
    {
        return (object2.transform.position - object1.transform.position).normalized;
    }

    /*
    protected List<GameObject> ObjectsWithTagInRange(List<GameObject> whichObjects, String withWhatTag, float inWhatRange, GameObject fromWhatObject)
    {
        List<GameObject> list = new List<GameObject>();
        whichObjects.ForEach(delegate (GameObject objectInList)
        {
            if (objectInList.GetComponent<Tags>().tags.Contains(withWhatTag))
            {
                if (DistancetoObject(objectInList,fromWhatObject) <= inWhatRange)
                {
                    if (objectInList != fromWhatObject)
                    {
                        list.Add(objectInList);
                    }
                }
            }
        });
        list.TrimExcess();
        return list;
    }
    */
}

