using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 50f;

    private Rigidbody body;
    private Vector3 moveInput;

    void Awake() {
        // grab this component's rigidbody object for position control
        body = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // read keyboard every frame: A/D or arrows = x, W/S or arrows = z
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        
        // turn our move input into a vector for physics force
        moveInput = new Vector3(x, 0f, z).normalized;
    }

    void FixedUpdate()
    {
        // adds velocity in the movement direction (* move speed scalar)
        body.AddForce(moveInput * moveSpeed, ForceMode.Acceleration);
    }
}
