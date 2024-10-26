using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceTracking : MonoBehaviour
{
    private WebCamTexture webcamTexture;

    // Start is called before the first frame update
    private void InitializeCamera()
    {
        var devices = WebCamTexture.devices;
        webcamTexture = new WebCamTexture(devices[0].name);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = webcamTexture;
        webcamTexture.Play();
    }

    void Start()
    {
        InitializeCamera();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
