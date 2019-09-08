using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngineEffect : MonoBehaviour
{
    public GameObject ship;
    private Light _light;
    private Rigidbody _rb;

    void Start()
    {
        _light = GetComponent<Light>();
        _rb = ship.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        _light.intensity = _rb.velocity.magnitude;
    }
}
