using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// hoge

[RequireComponent(typeof(Rigidbody))]
public class SimpleWing : MonoBehaviour
{
    public static bool enableLift = true;
    public static bool enableDrag = true;

    [Header("Surface Area (Estimate from BoxCollider)")]
    //Wing area 翼面積 [m^2]
    public float wingArea = 20;

    //追加の揚力係数（動翼の効果）
    public float additionalCoefficientOfLift;
    //追加の抗力係数（動翼の効果）
    public float additionalCoefficientOfDrag;

    public Vector3 currentLift { get; private set; }
    public Vector3 currentDrag { get; private set; }
    public float currentAoa { get; private set; }
    public float currentSpeed { get; private set; }

    private void Reset()
    {
        var boxCollider = GetComponent<BoxCollider>();
        wingArea = boxCollider.size.x * boxCollider.size.z;
    }


    private void FixedUpdate()
    {
        Rigidbody rigidBody = this.GetComponent<Rigidbody>();

        //Convert to local vector ローカルなベクトルに変換
        Vector3 localVelocity = this.transform.InverseTransformVector(rigidBody.velocity);

        //Speed 対気速度
        float v = new Vector3(0, localVelocity.y, localVelocity.z).magnitude;

        //Get Angle-of-attack(pitch) from local vector ローカルなベクトルから迎角(ピッチ角)を計算
        float aoa = -Mathf.Atan2(localVelocity.y, localVelocity.z) * Mathf.Rad2Deg;

        //Lift coefficient 揚力係数
        float cl = (aoa * 0.1f) + additionalCoefficientOfLift;
        
        if (!enableLift) cl = 0;

        //Calculate lift 揚力を計算
        float lift = CalculateLiftOrDrag(cl, wingArea, v);

        //Calculate lift direction (normalized) vector 揚力ベクトルを計算
        Vector3 liftVector = Vector3.Cross(Vector3.Cross(rigidBody.velocity, this.transform.up), rigidBody.velocity).normalized;

        //Apply lift 揚力を適用
        rigidBody.AddForce(liftVector * lift);

        Debug.DrawLine(this.transform.position, this.transform.position + liftVector * lift * 0.001f);

        //Drag coefficient 抗力係数
        float cd = Mathf.Clamp01(0.005f + Mathf.Pow(Mathf.Abs(aoa)*0.0315f,5)) + additionalCoefficientOfDrag;

        if (!enableDrag) cd = 0;

        //Calculate drag 抗力を計算
        float drag = CalculateLiftOrDrag(cd, wingArea, v);

        //Calculate drag direction (normalized) vector 抗力ベクトルを計算
        Vector3 dragVector = -rigidBody.velocity.normalized; //単にベロシティの逆を正規化するだけ

        //Apply drag 揚力を適用
        rigidBody.AddForce(dragVector * drag);

        //動翼による追加の揚力・抗力係数をリセット
        additionalCoefficientOfLift = 0;
        additionalCoefficientOfDrag = 0;

        //Visualizer用に外部から参照できる値
        currentLift = liftVector * lift;
        currentDrag = dragVector * drag;
        currentAoa = aoa;
        currentSpeed = localVelocity.z;

    }


    /// <summary> Calculate lift(or drag) 揚力/抗力を計算 </summary>
    /// <returns>The lift or drag[N]</returns>
    /// <param name="coefficient"> Lift or Drag coefficient </param>
    /// <param name="wingArea"> Surface area [m^2] </param>
    /// <param name="speed"> Speed [m/s] </param>
    /// <param name="airDensity"> Air density [kg/m^3]. </param>
    public static float CalculateLiftOrDrag(float coefficient, float surface, float velocity, float airDensity = 1.293f)
    {
        float q = 0.5f * (velocity * velocity) * airDensity;//動圧
        return q * surface * coefficient;
    }

}
