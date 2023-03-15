using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlMovement : MonoBehaviour
{
    public Rigidbody rb;
    public float speed;

    private float hor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");

    }

    void FixedUpdate()
    {
        rb.AddForce(hor * speed, 0, 0);
    }
}
