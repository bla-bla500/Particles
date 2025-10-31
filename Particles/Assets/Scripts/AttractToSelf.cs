using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class AttractToSelf : ParticleManager
{
    private Rigidbody2D thisObjectsRigidbody;
    private Collider2D[] thisObjectsColliders;
    private List<GameObject> objectsInRange;
    [SerializeField] private string attractToWhat;
    [SerializeField] private float range;
    [SerializeField] private float attractionStrengthMultiplier;
    [SerializeField] private float repelForceRange;
    [SerializeField] private float repelForceMultiplier;
    [SerializeField] private float dampingMultiplier;
    private float radious;

    void Start()
    {
        allParticlesOnScreen.Add(gameObject);

        //Disables Rigidbodies Collider
        thisObjectsRigidbody = gameObject.GetComponent<Rigidbody2D>();
        thisObjectsColliders = new Collider2D[2];
        thisObjectsRigidbody.GetAttachedColliders(thisObjectsColliders);
        for (int i = 0; i == thisObjectsColliders.Length -1; i++)
        {
            thisObjectsColliders[i].enabled = false;
        }
        //Gets this particles size
        radious = 0.2f;
    }
    void FixedUpdate()
    {
        objectsInRange = ObjectsWithTagInRange(allParticlesOnScreen, attractToWhat, range, gameObject);
        for (int i = 0;i < objectsInRange.Count; i++)
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

            thisObjectsRigidbody.AddForce(endingForce * globalSpeed, ForceMode2D.Impulse);
        }
    }

    private void OnDestroy()
    {
        allParticlesOnScreen.Remove(gameObject);
        allParticlesOnScreen.TrimExcess();
    }

}
