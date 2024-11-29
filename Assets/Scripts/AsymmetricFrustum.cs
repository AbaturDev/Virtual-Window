using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Emgu.CV.Fuzzy.FuzzyInvoke;


[RequireComponent(typeof(Camera))]
[ExecuteInEditMode]
public class AsymmetricFrustum : MonoBehaviour
{
    public GameObject virtualWindow;

    public float width = 60;    //in cm

    public float height = 34;   //in cm

    private float windowHeight;
    private float windowWidth;
    public float maxHeight = 2000.0f;

    private bool verbose = true;

    public void SetAsymFrustum(Camera cam, Vector3 position, float nearestClip)
    {

        // Focal length = orthogonal distance to image plane
        Vector3 newPosition = position;
        //newpos.Scale (new Vector3 (1, 1, 1));
        float focal = 1;

        newPosition = new Vector3(newPosition.x, newPosition.z, newPosition.y);
        if (verbose)
        {
            Debug.Log(newPosition.x + ";" + newPosition.y + ";" + newPosition.z);
        }

        focal = Mathf.Clamp(newPosition.z, 0.001f, maxHeight);

        // Ratio for intercept theorem
        float ratio = focal / nearestClip;

        // Compute size for focal
        float imageLeft = (-windowWidth / 2.0f) - newPosition.x;
        float imageRight = (windowWidth / 2.0f) - newPosition.x;
        float imageTop = (windowHeight / 2.0f) - newPosition.y;
        float imageBottom = (-windowHeight / 2.0f) - newPosition.y;

        // Intercept theorem
        float nearLeft = imageLeft / ratio;
        float nearRight = imageRight / ratio;
        float nearTop = imageTop / ratio;
        float nearBottom = imageBottom / ratio;

        Matrix4x4 m = PerspectiveOffCenter(nearLeft, nearRight, nearBottom, nearTop, cam.nearClipPlane, cam.farClipPlane);
        cam.projectionMatrix = m;
    }

    static Matrix4x4 PerspectiveOffCenter( float left, float right, float bottom, float top, float near, float far)
    {
        var x = (2.0f * near) / (right - left);
        var y = (2.0f * near) / (top - bottom);
        var a = (right + left) / (right - left);
        var b  = (top + bottom) / (top - bottom);
        var c  = -(far + near) / (far - near);
        var d  = -(2.0f * far* near) / (far - near);
        var e  = -1.0f;

        var m = new Matrix4x4();
        m[0, 0] = x; m[0, 1] = 0; m[0, 2] = a; m[0, 3] = 0;
        m[1, 0] = 0; m[1, 1] = y; m[1, 2] = b; m[1, 3] = 0;
        m[2, 0] = 0; m[2, 1] = 0; m[2, 2] = c; m[2, 3] = d;
        m[3, 0] = 0; m[3, 1] = 0; m[3, 2] = e; m[3, 3] = 0;
        return m;
    }

    void LateUpdate()
    {
        windowHeight = height;
        windowWidth = width;

        Vector3 cameraPosition = virtualWindow.transform.InverseTransformPoint(transform.position);

        SetAsymFrustum(GetComponent<Camera>(), cameraPosition, GetComponent<Camera>().nearClipPlane);
    }
}
