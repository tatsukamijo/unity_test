using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class SimpleUIShower : MonoBehaviour
{
    public Text speedText;
    public Text altitudeText;
    public Text aoaText;
    public Text thrustText;
    public Text flapAngleText;
    public Text weightText;

    public Toggle toggleLift;
    public Toggle toggleDrag;
    public Toggle toggleBreak;

    public Toggle toggleShowLiftLine;
    public Toggle toggleShowDragLine;
    public Toggle toggleShowCenterOfMass;

    [Header("Data source | データの採取元")]
    public SimplePropeller propeller;
    public SimpleWing mainWing;
    public SimpleControlSurface flap;

    public Rigidbody[] parts;

    public GameObject centerOfMassObject;

    Vector3 CalculateCenterOfGravity(out float mass)
    {
        Vector3 com = Vector3.zero;
        float c = 0f;
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].GetComponent<Joint>() || parts[i].GetComponent<SimpleBody>())
            {
                com += parts[i].worldCenterOfMass * parts[i].mass;
                c += parts[i].mass;
            }
        }
        com /= c;
        mass = c;
        return com;
    }
    void SetBreakMode(bool breakable)
    {
        for (int i = 0; i < parts.Length; i++)
        {
            var joint = parts[i].GetComponent<Joint>();
            if (joint) joint.breakForce = breakable ? 60000 : float.PositiveInfinity;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        toggleLift.onValueChanged.AddListener((value) => { SimpleWing.enableLift = value; });
        toggleDrag.onValueChanged.AddListener((value) => { SimpleWing.enableDrag = value; });
        toggleBreak.onValueChanged.AddListener((value) => { SetBreakMode(value); });
        toggleShowLiftLine.onValueChanged.AddListener((value) => { SimpleWingVisualizer.showLift = value; });
        toggleShowDragLine.onValueChanged.AddListener((value) => { SimpleWingVisualizer.showDrag = value; });
        toggleShowCenterOfMass.onValueChanged.AddListener((value) => { centerOfMassObject.SetActive(value); });
    }

    // Update is called once per frame
    void Update()
    {
        var body = propeller.GetComponentInParent<Rigidbody>();

        //パワー表示
        thrustText.text = (propeller.currentInput * 100).ToString("f0") + "% (" + (propeller.power * propeller.currentInput).ToString("f0") + "N)";

        //速度表示
        string knot = (mainWing.currentSpeed * 1.94384f).ToString("f1");
        string kmh = (mainWing.currentSpeed * 3.6f).ToString("f1");
        speedText.text = knot + "knot (" + kmh + "km/h)";

        //高度表示
        float altitude = float.NaN;
        if (Physics.Raycast(body.transform.position, -Vector3.up, out RaycastHit hitinfo, 50000))
        {
            altitude = hitinfo.distance;
        }
        string feet = (altitude * 3.28084f).ToString("f0");
        altitudeText.text = feet + "ft(" + altitude.ToString("f1") + "m)";

        //フラップ角度の表示
        flapAngleText.text = Mathf.Abs(flap.currentAngle).ToString("f0") + "deg (Max" + flap.maxDeflectionAngle.ToString("f0")+")";

        //AoAの表示
        aoaText.text = mainWing.currentAoa.ToString("f1") + "deg";

        //質量中心の計算
        Vector3 centerOfMass = CalculateCenterOfGravity(out float weightSum);

        //質量合計の表示
        weightText.text = weightSum + "kg";

        //質量中心を球体で表示
        Vector3 com2Cam = Camera.main.transform.position - centerOfMass;
        centerOfMassObject.transform.position = centerOfMass + com2Cam * 0.5f;
    }


}
