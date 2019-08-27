using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Player : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        ProceduralCube cube = gameObject.AddComponent<ProceduralCube>();
        cube.transform.Translate(new Vector3(-2, 0, 0));

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.convex = true;

        rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorz = Input.GetAxis("Horizontal");
        float moveVert = Input.GetAxis("Vertical");

        Vector3 force = new Vector3(moveHorz * speed, 0, moveVert * speed);

        rb.AddForce(force);
    }
}
