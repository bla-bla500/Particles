using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Unity.VisualScripting;
using Unity.Mathematics;
using UnityEngine;

public class AttractToSelf : ParticleManager
{
    // attract to varibles; runs on CPU
    /*
    [SerializeField] private string attractToWhat;
    [SerializeField] private float range;
    [SerializeField] private float attractionStrengthMultiplier;
    [SerializeField] private float repelForceRange;
    [SerializeField] private float repelForceMultiplier;
    [SerializeField] private float dampingMultiplier;
    [SerializeField] private float radious;
    */


    private Rigidbody2D thisObjectsRigidbody;
    private Collider2D[] thisObjectsColliders;
    //private List<GameObject> objectsInRange;
    private int IDinShader;
    Predicate<GameObject> isThisGameObject;

    void Start()
    {
        isThisGameObject = obj => obj.GetInstanceID() == gameObject.GetInstanceID();
        GlobalValues.allParticlesOnScreen.Add(gameObject);

        FindIndex();

        //Disables Rigidbodies Collider
        thisObjectsRigidbody = gameObject.GetComponent<Rigidbody2D>();
        thisObjectsColliders = new Collider2D[2];
        thisObjectsRigidbody.GetAttachedColliders(thisObjectsColliders);
        for (int i = 0; i == thisObjectsColliders.Length -1; i++)
        {
            thisObjectsColliders[i].enabled = false;
        }

        //radious = 0.2f;
    }

    float dampingMultiplier = GlobalValues.dampingMultiplier;
    void FixedUpdate()
    {
        if (GlobalValues.computeShaderResults[IDinShader].z != 0)
        {
            thisObjectsRigidbody.AddForce(new Vector2(GlobalValues.computeShaderResults[IDinShader].x, GlobalValues.computeShaderResults[IDinShader].y) * GlobalValues.globalSpeed, ForceMode2D.Impulse);
        }

        thisObjectsRigidbody.linearVelocity = thisObjectsRigidbody.linearVelocity * dampingMultiplier;

        /* attract to; runs on CPU (bad code)
        objectsInRange = ObjectsWithTagInRange(GlobalValues.allParticlesOnScreen, attractToWhat, range, gameObject);
        for (int i = 0; i < objectsInRange.Count; i++)
        {
            //Moves tords objects
            float distance = DistancetoObject(gameObject, objectsInRange[i]);
            float howClose = -distance;
            howClose = (howClose + range)*attractionStrengthMultiplier;
            Vector2 endingForce = DirectionToObject(gameObject, objectsInRange[i]) * howClose;

            //repels if to close
            if (distance < radious + repelForceRange)
            {
                endingForce = endingForce + (DirectionToObject(gameObject, objectsInRange[i]) * -1) * (repelForceMultiplier/(float)Math.Pow(2, (distance*10)));
            }

            //damping

            Vector2 damping = (-dampingMultiplier) * new Vector2 ((float)Math.Pow(endingForce.x, 2) * endingForce.x, (float)Math.Pow(endingForce.y, 2) * endingForce.y);
            endingForce = endingForce + damping;

            thisObjectsRigidbody.AddForce(endingForce * GlobalValues.globalSpeed, ForceMode2D.Impulse);

        }
        */

    }

    private void OnDestroy()
    {
        GlobalValues.allParticlesOnScreen.RemoveAt(GlobalValues.allParticlesOnScreen.FindIndex(isThisGameObject));
        GlobalValues.allParticlesOnScreen.TrimExcess();
    }

    public void FindIndex()
    {
        IDinShader = GlobalValues.allParticlesOnScreen.FindIndex(isThisGameObject);
    }


}
