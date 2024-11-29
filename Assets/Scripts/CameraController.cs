using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public FaceDetector faceDetector;
    public float movementScaleX = 0.005f;
    public float movementScaleY = 0.005f;
    public float movementScaleZ = 0.01f;

    public float rotationScaleX = 0.01f;
    public float rotationScaleY = 0.01f;

    private float minYPosition = 1.0f;

    private float smoothSpeed = 3f;
    private Vector3 initCameraPosition;
    private Quaternion initCameraRotation;

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

        Vector3 targetPosition = initCameraPosition + new Vector3(offSetX, -offSetY, -offSetZ);

        if(targetPosition.y < minYPosition)
        {
            targetPosition.y = minYPosition;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);    //linear interpolation

        float rotationX = -(facePosition.y - 240) * rotationScaleY;
        float rotationY = -(facePosition.x - 320) * rotationScaleX;

        Quaternion targetRotation = Quaternion.Euler(initCameraRotation.eulerAngles.x + rotationX, initCameraRotation.eulerAngles.y + rotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * smoothSpeed);

    }
}
