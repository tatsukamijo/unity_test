using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToggle : MonoBehaviour
{
    public Camera[] cameras;
    int current;

    void DeactiveAll()
    {
        for (int i = 0; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        DeactiveAll();
        cameras[0].gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            current++;
            if (current >= cameras.Length) current = 0;

            DeactiveAll();
            cameras[current].gameObject.SetActive(true);
        }
    }
}
