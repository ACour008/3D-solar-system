using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum OrbitalType
{
    NONE,
    STAR,
    PLANET,
    MOON,
    ASTEROID,
    PORT
}

public class Orbital : GravityObject
{
    [HideInInspector] public float mass;
    public float radius;
    public float surfaceGravity;
    public Vector3 initialVelocity;
    public Vector3 currentVelocity;
    public string orbitalName;

    public OrbitalType orbitalType;

    [HideInInspector] public Transform meshHolder;

    [HideInInspector] public Rigidbody rb;

    void Awake() 
    {
        currentVelocity = initialVelocity;
        rb = GetComponent<Rigidbody>();
        rb.mass = mass;
    }

    public void RefreshVelocity(Orbital[] allBodies, float timeStep)
    {
        for(int i = 0; i < allBodies.Length; i++)
        {
            Orbital other = allBodies[i];
            if (other != this)
            {
                float sqrDist = (other.rb.position - rb.position).sqrMagnitude;
                Vector3 forceDir = (other.rb.position - rb.position).normalized;
                Vector3 force = forceDir * Universe.GRAVITATIONAL_CONSTANT * mass * other.mass / sqrDist;
                Vector3 acceleration = force / mass;
                currentVelocity += acceleration * timeStep;
            }
        }
    }

    public void RefeshVelocity(Vector3 acceleration, float timeStep)
    {
        currentVelocity += acceleration * timeStep;
    }

    public void RefreshPosition(float timeStep) {
        rb.MovePosition(rb.position + currentVelocity * timeStep);
    }

    void OnValidate()
    {
        mass = surfaceGravity * radius * radius / Universe.GRAVITATIONAL_CONSTANT;
        meshHolder = transform;
        meshHolder.localScale = Vector3.one * radius;
        gameObject.name = orbitalName;
    }
}
