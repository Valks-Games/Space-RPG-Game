using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Player : MonoBehaviour
{
    public float speed = 1f;
    public float jumpForce = 500f;

    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private float adjSpeed;
    private bool jump;

    private void Awake()
    {
        adjSpeed = speed * 1000f;
    }

    private void Start()
    {
        InitPlayerObject();
        InitMeshRenderer();
        InitRigidBody();
        InitCollider();
    }

    private void InitMeshRenderer() {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = (Material)Resources.Load("Materials/Player");
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
        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.freezeRotation = true;
        rb.drag = 1f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            jump = true;
    }

    private void FixedUpdate()
    {
        float moveHorz = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");

        Vector3 moveForce = new Vector3(moveHorz * adjSpeed * Time.deltaTime, 0, moveVert * adjSpeed * Time.deltaTime);

        rb.AddForce(moveForce);

        if (jump)
        {
            if (IsGrounded())
                rb.AddForce(new Vector3(0, jumpForce, 0));
            jump = false;
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
