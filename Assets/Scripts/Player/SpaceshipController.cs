using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public Camera FrontCamera;
    public Camera BackCamera;
    public GameObject actual_model; //"Ship GameObject", "Point this to the Game Object that actually contains the mesh for the ship. Generally, this is the first child of the empty container object this controller is placed in."

    public float turnspeed = 15.0f; //"Turn/Roll Speed", "How fast turns and rolls will be executed "
    public float Torque = 10f;

    private Transform _myT;
    private Rigidbody _rb;
    [HideInInspector]
    public bool afterburner_Active = false; //True if afterburners are on.
    [HideInInspector]
    public bool slow_Active = false; //True if brakes are on

    public float pitchYaw_strength = 0.5f; //"Pitch/Yaw Multiplier", "Controls the intensity of pitch and yaw inputs"

    float distFromVertical; //Distance in pixels from the vertical center of the screen.
    float distFromHorizontal; //Distance in pixel from the horizontal center of the screen.

    public float screen_clamp = 500; //"Screen Clamp (Pixels)", "Once the pointer is more than this many pixels from the center, the input in that direction(s) will be treated as the maximum value."

    [HideInInspector]
    public float roll, yaw, pitch; //Inputs for roll, yaw, and pitch, taken from Unity's input system.

    public float afterburner_speed = 40f; //Afterburner Speed", "Speed when the button for positive thrust is being held down"
    public float thrust_transition_speed = 5f; //Thrust Transition Speed", "How quickly afterburners/brakes will reach their maximum effect"
    public float speed = 20.0f; //"Base Speed", "Primary flight speed, without afterburners or brakes"
    public float slow_speed = 4f; //"Brake Speed", "Speed when the button for negative thrust is being held down"
    Vector2 mousePos = new Vector2(0, 0); //Pointer position from CustomPointer
    public float bank_angle_clamp = 360; //"Bank Angle Clamp", "Maximum angle the spacecraft can rotate along the Z axis."
    public float bank_rotation_speed = 3f; //"Bank Rotation Speed", "Rotation speed along the Z axis when yaw is applied. Higher values will result in snappier banking."
    public float bank_rotation_multiplier = 1f; //"Bank Rotation Multiplier", "Bank amount along the Z axis when yaw is applied."

    float DZ = 0;
    float currentMag = 0;

    public float RollSpeed = 0.5f;

    public void Start()
    {
        mousePos = new Vector2(0, 0);

        _myT = transform;
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;

        DZ = CustomPointer.instance.deadzone_radius;
    }

    public void Update()
    {
        HandleView();
    }

    public void FixedUpdate()
    {
        Movement();
    }

    private void HandleView()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            FrontCamera.enabled = false;
            BackCamera.enabled = true;
        }
        else
        {
            FrontCamera.enabled = true;
            BackCamera.enabled = false;
        }
    }

    private void Movement()
    {
        _myT.Rotate(new Vector3(Input.GetAxis("Roll") * RollSpeed, 0, 0));

        updateCursorPosition();

        //Getting the current speed.
        currentMag = GetComponent<Rigidbody>().velocity.magnitude;

        if (Input.GetAxis("Thrust") > 0)
        {
            currentMag = Mathf.Lerp(currentMag, afterburner_speed, thrust_transition_speed * Time.deltaTime);
        }
        else if (Input.GetAxis("Thrust") < 0)
        {   //If input on the thrust axis is negatve, activate brakes.
            currentMag = Mathf.Lerp(currentMag, slow_speed, thrust_transition_speed * Time.deltaTime);
        }
        else
        { //Otherwise, hold normal speed.
            currentMag = Mathf.Lerp(currentMag, speed, thrust_transition_speed * Time.deltaTime);
        }

        pitch = Mathf.Clamp(distFromVertical, -screen_clamp - DZ, screen_clamp + DZ) * pitchYaw_strength;
        yaw = Mathf.Clamp(distFromHorizontal, -screen_clamp - DZ, screen_clamp + DZ) * pitchYaw_strength;
        float roll = turnspeed * Time.deltaTime * Input.GetAxis("Roll");

        _rb.AddRelativeTorque(roll, yaw, pitch);
        _rb.velocity = -1 * transform.right * currentMag;
    }

    void updateCursorPosition()
    {

        mousePos = CustomPointer.pointerPosition;

        //Calculate distances from the center of the screen.
        float distV = Vector2.Distance(mousePos, new Vector2(mousePos.x, Screen.height / 2));
        float distH = Vector2.Distance(mousePos, new Vector2(Screen.width / 2, mousePos.y));

        //If the distances are less than the deadzone, then we want it to default to 0 so that no movements will occur.
        if (Mathf.Abs(distV) < DZ)
            distV = 0;
        else
            distV -= DZ;
        //Subtracting the deadzone from the distance. If we didn't do this, there would be a snap as it tries to go to from 0 to the end of the deadzone, resulting in jerky movement.

        if (Mathf.Abs(distH) < DZ)
            distH = 0;
        else
            distH -= DZ;

        //Clamping distances to the screen bounds.	
        distFromVertical = Mathf.Clamp(distV, 0, (Screen.height));
        distFromHorizontal = Mathf.Clamp(distH, 0, (Screen.width));

        //If the mouse position is to the left, then we want the distance to go negative so it'll move left.
        if (mousePos.x < Screen.width / 2 && distFromHorizontal != 0)
        {
            distFromHorizontal *= -1;
        }
        //If the mouse position is above the center, then we want the distance to go negative so it'll move upwards.
        if (mousePos.y >= Screen.height / 2 && distFromVertical != 0)
        {
            distFromVertical *= -1;
        }


    }
}
