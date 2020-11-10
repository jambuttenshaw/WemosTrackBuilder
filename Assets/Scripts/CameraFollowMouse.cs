using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowMouse : MonoBehaviour
{

    public Vector3 initalPosition;
    public float smoothing;
    public float threshold;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if ((mouseWorldPos - transform.position).sqrMagnitude > threshold * threshold)
            transform.position = Vector3.Lerp(transform.position, mouseWorldPos, smoothing * Time.deltaTime);
    }
}
