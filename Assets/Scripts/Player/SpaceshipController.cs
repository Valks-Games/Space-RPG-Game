using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public Camera FrontCamera;
    public Camera BackCamera;

    public float TurnSpeed = 25f;

    private Transform _myT;
    private Rigidbody _rb;

    private float _velocity = 0f;
    private float _acc = 0f;

    public void Start()
    {
        _myT = transform;
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    public void Update()
    {
        HandleView();
        Turn();
        Thrust();
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

    private void Turn()
    {
        float yaw = TurnSpeed * Time.deltaTime * Input.GetAxis("Horizontal");
        float pitch = TurnSpeed * Time.deltaTime * Input.GetAxis("Pitch");
        float roll = TurnSpeed * Time.deltaTime * Input.GetAxis("Roll");

        Quaternion rotation = Quaternion.Euler(roll, yaw, pitch);
        _rb.MoveRotation(_rb.rotation * rotation);
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            _acc += 0.01f;

        _acc -= 0.005f;
        _acc = Mathf.Clamp(_acc, 0, 1);

        _velocity += _acc;

        Debug.Log(_velocity);

        _rb.AddForce(-1 * _myT.right * _velocity * Time.deltaTime);
    }
}
