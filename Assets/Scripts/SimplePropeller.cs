using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePropeller : MonoBehaviour
{
    //Thrust power[N]
    //����[�P�ʁF�j���[�g��]
    public float power = 3000;

    //Thrust power[N]
    //����[�P�ʁF�j���[�g��]
    public float currentInput;

    Rigidbody body => GetComponentInParent<Rigidbody>();

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            currentInput += 0.01f;
        }
        if (Input.GetButton("Fire2"))
        {
            currentInput -= 0.01f;
        }
        currentInput = Mathf.Clamp01(currentInput);

        this.transform.localRotation *= Quaternion.Euler(0,0,50 * currentInput);
    }

    private void FixedUpdate()
    {
        body.AddForce(body.transform.forward * power * currentInput);
    }
}
