using UnityEngine;
using System.Collections;

public class cameraFOV : MonoBehaviour
{
    public Camera mainCam;
    public float speed;
    public float maxFov;
    public float minFov;

    private float newFov;
    private float margin;

    void Start()
    {
        newFov = minFov;
        margin = 5f;
    }

    void Update()
    {
        mainCam.fieldOfView = Mathf.Lerp(mainCam.fieldOfView, newFov, speed * Time.deltaTime);
        //mainCam.transform.position += Vector3.Lerp(mainCam.transform.position, mainCam.transform.position + new Vector3(0.0f, 10.0f, 0.0f), speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("player"))
        {
            if (mainCam.fieldOfView < maxFov - margin) newFov = maxFov;
            else newFov = minFov;
        }
    }
}