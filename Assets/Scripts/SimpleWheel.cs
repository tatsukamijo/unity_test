using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWheel : MonoBehaviour
{
    Rigidbody rb => GetComponent<Rigidbody>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        var localVelocity = rb.transform.InverseTransformVector(rb.velocity);
        float brake = Input.GetKey(KeyCode.B) ? 1 : 0;
        rb.AddRelativeForce(-localVelocity.x * rb.mass, 0, 0);
        rb.AddForce(-rb.velocity.normalized * brake * rb.mass * 450);
    }
}
