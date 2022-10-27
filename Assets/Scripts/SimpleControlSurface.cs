using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-100)]
public class SimpleControlSurface : MonoBehaviour
{
    public SimpleWing wing;

    public float maxDeflectionAngle = 15;

    public float clPerDegree = 0.017f;
    public float cdPerDegree = 0.0015f;

    [Header("Input Setting")]
    public string axisName = "Horizontal";
    public bool invert;
    public bool flapMode = false;
    float sign => invert ? -1 : 1;

    float input = 0;

    public float currentAngle { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        wing = GetComponentInParent<SimpleWing>();
        StartCoroutine(LateFixedUpdate());
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (flapMode)
        {
            if (Input.GetKey(KeyCode.F)) input += 0.0025f;
            if (Input.GetKey(KeyCode.R)) input -= 0.0025f;
            input = Mathf.Clamp01(input);
        }
        else
        {
            input = Input.GetAxis(axisName);
        }

        currentAngle = -input * sign * maxDeflectionAngle;

        //ÉÅÉbÉVÉÖÇå©ÇΩñ⁄è„Ç≈âÒì]Ç≥ÇπÇÈ
        this.transform.localRotation = Quaternion.Euler(-input * sign * maxDeflectionAngle, 0, 0);
    }

    private void FixedUpdate()
    {
        wing.additionalCoefficientOfLift += input * maxDeflectionAngle * clPerDegree * sign;
        wing.additionalCoefficientOfDrag += Mathf.Abs(input * maxDeflectionAngle * cdPerDegree * sign);
    }


    IEnumerator LateFixedUpdate()
    {
        while (enabled)
        {
            //Debug.Log(gameObject.name + " LFU " + Time.frameCount);
            //ógóÕåWêîÇóÉÇ…í«â¡Ç∑ÇÈ

            yield return new WaitForFixedUpdate();
        }

    }
}
