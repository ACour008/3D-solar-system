using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct VelocityData
{
    public float maxSpeed;
    public float accelTime;
    public float decelTime;
    public float brakeTime;
}

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private InputHandler input;

    public bool invertPitch;
    public bool invertRoll;
    public bool invertYaw;

    public VelocityData thrust;
    public Vector3 angularThrust;


    [Header("VELOCITY INFO")]
    public float acceleration;
    public float deceleration;
    public float brakeRate;
    public float forwardVelocity;
    public Vector3 angularAccelRates;
    public Vector3 targetVelocity;
    public Vector3 currentVelocity;
    public Vector3 directions;
    


    void Awake()
    {
        input = GetComponent<InputHandler>();
        rb = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        ReadInput();
    }

    public void LateRefresh()
    {
        HandleMovement();
    }

    private void AddToVelocity(float rate, float minVelocity, float maxVelocity, float deltaTime)
    {
        forwardVelocity += rate * deltaTime;
        forwardVelocity = Mathf.Clamp(forwardVelocity, minVelocity, maxVelocity);
    }

    private void ReadInput()
    {
        if (input.ThrustActive) AddToVelocity(acceleration, -thrust.maxSpeed, thrust.maxSpeed, Time.deltaTime);
        if (input.BrakeActive) AddToVelocity(brakeRate, -thrust.maxSpeed, thrust.maxSpeed, Time.deltaTime);
    
        targetVelocity = new Vector3(input.Pitch * angularThrust.x, input.Yaw * angularThrust.y, input.Roll * angularThrust.z);
        currentVelocity = transform.InverseTransformDirection(rb.angularVelocity);

        if( (currentVelocity.x != targetVelocity.x))
        {
            if(Mathf.Abs(targetVelocity.x - currentVelocity.x) < angularAccelRates.x * Time.deltaTime * 1)
            {
                directions.x = 0;
                currentVelocity.x = targetVelocity.x;
            }
            else
                directions.x = Mathf.Sign(targetVelocity.x - currentVelocity.x);
        }

        if( (currentVelocity.y != targetVelocity.y))
        {
            if(Mathf.Abs(targetVelocity.y - currentVelocity.y) < angularAccelRates.y * Time.deltaTime * 1)
            {
                directions.y = 0;
                currentVelocity.y = targetVelocity.y;
            }
            else
                directions.y = Mathf.Sign(targetVelocity.y - currentVelocity.y);
        }

        if( (currentVelocity.z != targetVelocity.z))
        {

            if(Mathf.Abs(targetVelocity.z - currentVelocity.z) < angularAccelRates.z * Time.deltaTime * 1)
            {
                directions.z = 0;
                currentVelocity.z = targetVelocity.z;
            }
            else
                directions.z = Mathf.Sign(targetVelocity.z - currentVelocity.z);
        }

        rb.angularVelocity = transform.TransformDirection(currentVelocity);
    }

    private void HandleMovement()
    {
        if (input.ThrustActive)
            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * forwardVelocity, Time.deltaTime);
        else
        {
            AddToVelocity(deceleration, 0, thrust.maxSpeed, Time.deltaTime);
            rb.velocity = Vector3.Lerp(rb.velocity, transform.forward * forwardVelocity, Time.deltaTime);
        }

        if (directions.x != 0)
            rb.AddTorque(directions.x * transform.TransformDirection(Vector3.right) * angularAccelRates.x * Time.deltaTime, ForceMode.Impulse);
        
        if (directions.y != 0)
            rb.AddTorque(directions.y * transform.TransformDirection(Vector3.up) * angularAccelRates.y * Time.deltaTime, ForceMode.Impulse);
        
        if (directions.z != 0)
            rb.AddTorque(directions.z * transform.TransformDirection(Vector3.forward) * angularAccelRates.z * Time.deltaTime, ForceMode.Impulse);
    }

    private void Initialize()
    {
        input.IsInvertedPitch = invertPitch;
        input.IsInvertedRoll = invertRoll;
        input.IsInvertedYaw = invertYaw;

        acceleration = thrust.maxSpeed / thrust.accelTime;
        deceleration = -thrust.maxSpeed / thrust.decelTime;
        brakeRate = -thrust.maxSpeed / thrust.brakeTime;
        forwardVelocity = 0;

        angularThrust = angularAccelRates / rb.mass;
        var shipExtents = transform.GetChild(0).GetComponent<MeshFilter>().mesh.bounds.extents;
        angularAccelRates.x = new Vector2(shipExtents.y, shipExtents.z).magnitude * angularThrust.x;
        angularAccelRates.y = new Vector2(shipExtents.x, shipExtents.z).magnitude * angularThrust.y;
        angularAccelRates.z = new Vector2(shipExtents.x, shipExtents.y).magnitude * angularThrust.z;
    }

    public void OnPositionChange(Vector3 newPosition)
    {
        rb.position -= newPosition;
    }
}