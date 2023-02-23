using UnityEngine;

public class PhysicsManager : MonoBehaviour
{
    public Orbital[] orbitals;


    void FixedUpdate()
    {
        for (int i = 0; i < orbitals.Length; i++)
        {
            Vector3 acceleration = CalculateAcceleration(orbitals[i].rb.position, orbitals[i]);
            orbitals[i].RefeshVelocity(acceleration, Universe.PHYSICS_TIMESTEP);
        }

        for (int i = 0; i < orbitals.Length; i++)
        {
            orbitals[i].RefreshPosition(Universe.PHYSICS_TIMESTEP);
        }
    }

    private Vector3 CalculateAcceleration(Vector3 point, Orbital current = null)
    {
        Vector3 acceleration = Vector3.zero;
        for(int i = 0; i < orbitals.Length; i++)
        {
            Orbital other = orbitals[i];
            if (other != current)
            {
                float sqrDist = (other.rb.position - point).sqrMagnitude;
                Vector3 forceDir = (other.rb.position - point).normalized;
                acceleration += forceDir * Universe.GRAVITATIONAL_CONSTANT * other.mass / sqrDist;
            }
        }
        return acceleration;
    }
}
