using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Player : MonoBehaviour
{
    public float Speed = 1f;
    public float JumpForce = 500f;

    private MeshRenderer _meshRenderer;
    private Rigidbody _rb;
    private float _adjSpeed;
    private bool _jump;

    private void Awake()
    {
        _adjSpeed = Speed * 1000f;
    }

    private void Start()
    {
        InitPlayerObject();
        InitMeshRenderer();
        InitRigidBody();
        InitCollider();
    }

    private void InitMeshRenderer() {
        _meshRenderer = GetComponent<MeshRenderer>();
        _meshRenderer.material = (Material)Resources.Load("Materials/Player");
    }

    private void InitPlayerObject() {
        ProceduralCube cube = gameObject.AddComponent<ProceduralCube>();
        cube.CreateCube(0.75f);
        cube.transform.Translate(new Vector3(0, 1f, 0));
    }

    private void InitCollider() {
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
    }

    private void InitRigidBody() {
        _rb = gameObject.AddComponent<Rigidbody>();
        _rb.useGravity = true;
        _rb.freezeRotation = true;
        _rb.drag = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            _jump = true;
    }

    private void FixedUpdate()
    {
        float moveHorz = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");

        Vector3 moveForce = new Vector3(moveHorz * _adjSpeed * Time.deltaTime, 0, moveVert * _adjSpeed * Time.deltaTime);

        _rb.AddForce(moveForce);

        if (_jump)
        {
            if (IsGrounded())
                _rb.AddForce(new Vector3(0, JumpForce, 0));
            _jump = false;
        }
    }

    private bool IsGrounded() {
        RaycastHit hit;
        Ray downRay = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(downRay, out hit)) {
            // Debug.Log(hit.triangleIndex);
            return hit.distance <= 0.375;
        }
            
        return false;
    }
}
