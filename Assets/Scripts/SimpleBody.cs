using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBody : MonoBehaviour
{
    public float coefficientOfDrag = 0.01f;
    public float weightShift;
    // Start is called before the first frame update


    private void OnValidate()
    {

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var rb = GetComponent<Rigidbody>();
        var capsuleCollider = GetComponent<CapsuleCollider>();

        float surfaceArea = capsuleCollider.radius * capsuleCollider.radius * 2;
        float drag = SimpleWing.CalculateLiftOrDrag(coefficientOfDrag,surfaceArea,rb.velocity.magnitude);

        rb.centerOfMass = new Vector3(0, 0, weightShift);

        rb.AddForce(-rb.velocity.normalized * drag);
    }
}
