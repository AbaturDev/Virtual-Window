using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public FaceDetector faceDetector;
    public float movementScaleX = 0.005f;
    public float movementScaleY = 0.005f;
    public float movementScaleZ = 0.1f;

    private Vector3 initCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        initCameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 facePosition = faceDetector.GetFacePosition();

        float offSetX = (facePosition.x - 320) * movementScaleX;
        float offSetY = (facePosition.y - 240) * movementScaleY;
        float offSetZ = (facePosition.z - 50) * movementScaleZ;

        transform.position = initCameraPosition + new Vector3(offSetX, -offSetY, -offSetZ);
    }
}
