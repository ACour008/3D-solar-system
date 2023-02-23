using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerControls controls;

    private bool _navSystemEngaged;
    private Vector2 screenCenter;
    public bool IsNavSystemsEngaged { get; private set; }
    public bool IsInvertedPitch { get; set; }
    public bool IsInvertedYaw   { get; set; }
    public bool IsInvertedRoll  { get; set; }

    public bool ThrustActive    { get; private set; }
    public bool BrakeActive     { get; private set; }
    public float Pitch { get; private set; }
    public float Yaw   { get; private set; }
    public float Roll  { get; private set; }


    void Awake()
    {
        controls = new PlayerControls();
    }

    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
        EnableSystems();
    }

    void OnDisable()
    {
        controls.Disable();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    public void EnableSystems()
    {
        controls.Systems.EngageNav.performed += OnNavigationEngaged;
    }

    public void EngageNavigation()
    {
        _navSystemEngaged = true;
        controls.Movement.Roll.performed += OnRollPerformed;
        controls.Movement.Thrust.performed += OnThrustPerformed;
        controls.Movement.Brake.performed += OnBreakPerformed;
        controls.Movement.Pitch.performed += OnPitchPerformed;
        controls.Movement.Yaw.performed += OnYawPerformed;
        controls.Movement.PitchYawMouse.performed += OnPitchYawPerformed;
    }

    public void DisengageNavigation()
    {
        _navSystemEngaged = false;
        controls.Movement.Roll.performed -= OnRollPerformed;
        controls.Movement.Thrust.performed -= OnThrustPerformed;
        controls.Movement.Brake.performed -= OnBreakPerformed;
        controls.Movement.Pitch.performed -= OnPitchPerformed;
        controls.Movement.Yaw.performed -= OnYawPerformed;
        controls.Movement.PitchYawMouse.performed -= OnPitchYawPerformed;
    }

    public void OnNavigationEngaged(InputAction.CallbackContext ctx)
    {
        if (IsNavSystemsEngaged)
        {
            Debug.Log("Nav Systems Deactivated");
            DisengageNavigation();
            IsNavSystemsEngaged = false;
        }
        else {
            Debug.Log("Nav System Activated");
            EngageNavigation();
            IsNavSystemsEngaged = true;
        }
    }

    public void OnThrustPerformed(InputAction.CallbackContext ctx) => ThrustActive = ctx.ReadValue<float>() > 0;

    public void OnBreakPerformed(InputAction.CallbackContext ctx) => BrakeActive = ctx.ReadValue<float>() > 0;

    public void OnPitchPerformed(InputAction.CallbackContext ctx)
    {
        float pitchValue = ctx.ReadValue<float>();
        Pitch = (IsInvertedPitch) ? pitchValue : -pitchValue;
    }

    public void OnYawPerformed(InputAction.CallbackContext ctx)
    {
        float yawValue = ctx.ReadValue<float>();
        Yaw = (IsInvertedYaw) ? -yawValue : yawValue;
    }

    public void OnRollPerformed(InputAction.CallbackContext ctx)
    {
        float rollValue = ctx.ReadValue<float>();
        Roll = (IsInvertedRoll) ? rollValue : -rollValue;
    }

    public void OnPitchYawPerformed(InputAction.CallbackContext ctx)
    {
        Vector2 mousePosition = ctx.ReadValue<Vector2>();
        Vector2 mouseFromScreen = (screenCenter - mousePosition).normalized;

        if( (screenCenter.y - mousePosition.y) < 95 || (screenCenter.y - mousePosition.y) > 230)        
            Pitch = (IsInvertedPitch) ? -mouseFromScreen.y : mouseFromScreen.y;
        else 
            Pitch = 0;

        if ((screenCenter.x - mousePosition.x) < -120 || (screenCenter.x - mousePosition.x) > 120)
        {
            Yaw = (IsInvertedYaw) ? mouseFromScreen.x : -mouseFromScreen.x;
        }
        else 
        {
            Yaw = 0;
        }
    }
}
