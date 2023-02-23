using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitTrajectory : MonoBehaviour
{
    public bool active;
    public int numSteps = 1000;
    public float timeStep = 0.1f;
    public bool usePhysicsTimeStep;

    public bool relativeToBody;
    public Orbital centralBody;
    public float width = 100;

    public bool preStartState;

    // Start is called before the first frame update
    void Start()
    {
        active = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (active) DrawOrbits();
        else HideOrbits();
    }

    void OnApplicationQuit()
    {
        active = preStartState;
    }

    void DrawOrbits()
    {
        Orbital[] orbitals = FindObjectsOfType<Orbital>();
        var virtualOrbitals = new VirtualOrbital[orbitals.Length];
        var drawPoints = new Vector3[orbitals.Length][];

        int referenceFrameIndex = 0;
        Vector3 referenceOrbitalInitialPosition = Vector3.zero;

        // Initialization of virtual orbitals
        for (int i = 0; i < virtualOrbitals.Length; i++)
        {
            virtualOrbitals[i] = new VirtualOrbital(orbitals[i]);
            drawPoints[i] = new Vector3[numSteps];

            if (orbitals[i] == centralBody && relativeToBody)
            {
                referenceFrameIndex = i;
                referenceOrbitalInitialPosition = virtualOrbitals[i].position;
            }
        }

        // Simulate
        for (int step = 0; step < numSteps; step++)
        {
            Vector3 referenceOrbitalPosition = (relativeToBody) ? virtualOrbitals[referenceFrameIndex].position : Vector3.zero;

            // update velocities (same as in Physics Manager)
            for (int i = 0; i < virtualOrbitals.Length; i++)
            {                
                virtualOrbitals[i].velocity += CalculateAcceleration(i, virtualOrbitals) * timeStep;
            }

            // update positions
            for (int i = 0; i < virtualOrbitals.Length; i++)
            {
                Vector3 newPosition = virtualOrbitals[i].position + virtualOrbitals[i].velocity * timeStep;
                virtualOrbitals[i].position = newPosition;
                if (relativeToBody)
                {
                    var referenceFrameOffset = referenceOrbitalPosition - referenceOrbitalInitialPosition;
                    newPosition -= referenceFrameOffset;
                }
                if (relativeToBody && i == referenceFrameIndex)
                {
                    newPosition = referenceOrbitalInitialPosition;
                }

                drawPoints[i][step] = newPosition;
            }
        }

        // Draw Paths
        for (int orbIndex = 0; orbIndex < virtualOrbitals.Length; orbIndex++)
        {
            var pathColor = DetermineColor(orbitals[orbIndex].orbitalType);

            var lineRenderer = orbitals[orbIndex].gameObject.GetComponent<LineRenderer>();
            lineRenderer.enabled = true;
            lineRenderer.positionCount = drawPoints[orbIndex].Length;
            lineRenderer.SetPositions(drawPoints[orbIndex]);
            lineRenderer.startColor = pathColor;
            lineRenderer.endColor = pathColor;
            lineRenderer.widthMultiplier = width;
        }
    }

    void HideOrbits()
    {
        Orbital[] orbitals = FindObjectsOfType<Orbital>();

        foreach(Orbital orbital in orbitals)
        {
            var lineRenderer = orbital.gameObject.GetComponent<LineRenderer>();
            lineRenderer.positionCount = 0;
        }
    }
    private Color DetermineColor(OrbitalType type)
    {
        Color color = Color.clear;
        switch(type)
        {
            case OrbitalType.STAR:
                color = Color.yellow;
                break;
            case OrbitalType.PLANET:
                color = Color.blue;
                break;
            case OrbitalType.MOON:
                color = Color.white;
                break;
            case OrbitalType.ASTEROID:
                color = Color.gray;
                break;
            case OrbitalType.PORT:
                color = Color.red;
                break;
        }

        return color;
    }

    private Vector3 CalculateAcceleration(int i, VirtualOrbital[] virtualOrbitals)
    {
        Vector3 acceleration = Vector3.zero;

        for (int j = 0; j < virtualOrbitals.Length; j++)
        {
            if (i == j) continue;

            Vector3 forceDir = (virtualOrbitals[j].position - virtualOrbitals[i].position).normalized;
            float sqrDist = (virtualOrbitals[j].position - virtualOrbitals[i].position).sqrMagnitude;
            acceleration += forceDir * Universe.GRAVITATIONAL_CONSTANT * virtualOrbitals[j].mass / sqrDist;
        }
        return acceleration;
    }

    void OnValidate()
    {
        if (usePhysicsTimeStep)
        {
            timeStep = Universe.PHYSICS_TIMESTEP;
            preStartState = active;
        }
    }
}

class VirtualOrbital
{
    public Vector3 position;
    public Vector3 velocity;
    public float mass;

    public VirtualOrbital(Orbital orbital)
    {
        position = orbital.transform.position;
        velocity = orbital.initialVelocity;
        mass = orbital.mass;
    }
}
