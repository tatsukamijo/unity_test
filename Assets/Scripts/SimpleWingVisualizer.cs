using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWingVisualizer : MonoBehaviour
{
    public static bool showLift = true;
    public static bool showDrag = true;

    public Material lineRendererMaterial;
    LineRenderer clLineRenderer;
    LineRenderer cdLineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        clLineRenderer = new GameObject("lineRenderer(cl)").AddComponent<LineRenderer>();
        clLineRenderer.material = lineRendererMaterial;
        cdLineRenderer = new GameObject("lineRenderer(cd)").AddComponent<LineRenderer>();
        cdLineRenderer.material = lineRendererMaterial;
    }

    private void LateUpdate()
    {
        var wing = GetComponent<SimpleWing>();
        var rb = GetComponent<Rigidbody>();
        var center = this.transform.position;

        var clLineEnd = center + wing.currentLift * 0.001f;
        var cdLineEnd = center + wing.currentDrag*0.01f;

        var absAoa = Mathf.Abs(wing.currentAoa);

        var liftColor = Color.Lerp(Color.cyan,Color.yellow, Mathf.InverseLerp(14,16, absAoa));
        liftColor = Color.Lerp(liftColor, Color.red, Mathf.InverseLerp(16, 18, absAoa));
        liftColor.a = 0.5f;

        var dragColor = Color.magenta;
        dragColor.a = 0.5f;

        if (!showLift) liftColor = Color.clear;
        if (!showDrag) dragColor = Color.clear;

        ShowLineInRuntime(clLineRenderer, center, clLineEnd, liftColor);
        ShowLineInRuntime(cdLineRenderer, center, cdLineEnd, dragColor);
    }

    static void ShowLineInRuntime(LineRenderer lineRenderer, Vector3 start, Vector3 end, Color color)
    {
        lineRenderer.SetPositions(new Vector3[] { start, end });
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = 0.03f;
        lineRenderer.endWidth = 0.03f;
    }
}
