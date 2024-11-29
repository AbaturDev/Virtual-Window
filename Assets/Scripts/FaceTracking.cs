using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.OCR;
using Emgu.CV.Tracking;

public class FaceDetector : MonoBehaviour
{
    private VideoCapture video;
    private Vector3 facePosition;

    private CascadeClassifier frontalFace;
    private CascadeClassifier eye;

    void Start()
    {
        video = new VideoCapture(0);
        if (video.IsOpened)
        {
            video.ImageGrabbed += OnImageGrabbed;
        }

        frontalFace = new CascadeClassifier(Application.dataPath + "/XML/haarcascade_frontalface_default.xml");
        eye = new CascadeClassifier(Application.dataPath + "/XML/haarcascade_eye.xml");

    }

    // Function for grabbing images
    void OnImageGrabbed(object sender, EventArgs args)
    {
        Mat source = new Mat();
        video.Retrieve(source);

        Image<Bgr, Byte> buffer_im = source.ToImage<Bgr, Byte>();
        source = buffer_im.Mat;

        CvInvoke.Flip(source, source, FlipType.Horizontal);

        // FACE DETECTION
        // Cloning the mat and converting it color to gray to use the CascadeClassifier
        Mat imgGray = source.Clone();
        CvInvoke.CvtColor(source, imgGray, ColorConversion.Bgr2Gray);

        Rectangle[] detectFaces = frontalFace.DetectMultiScale(imgGray);
        if (detectFaces.Length == 0)
        {
            return;
        }

        // Choosing the biggest face
        Rectangle mainFace = new Rectangle();
        foreach (var face in detectFaces)
        {
            if (face.Height * face.Width > mainFace.Height * mainFace.Width)
            {
                mainFace = face;
            }
        }
        CvInvoke.Rectangle(source, mainFace, new MCvScalar(0, 255, 0));

        //// EYE DETECTION
        //// Selecting the area where the face is located
        //Mat faceRegion = new Mat(imgGray, mainFace);
        //var detectedEyes = eye.DetectMultiScale(faceRegion);

        //foreach (var eye_ in detectedEyes)
        //{
        //    Rectangle eyePosition = new Rectangle(
        //        mainFace.X + eye_.X,
        //        mainFace.Y + eye_.Y,
        //        eye_.Width,
        //        eye_.Height);

        //    CvInvoke.Rectangle(source, eyePosition, new MCvScalar(255, 0, 255));
        //}

        float faceReferenceWidth = 300f;
        float referenceDistance = 50f;

        // Counting z position based on face proportion
        float currentFaceWidth = mainFace.Width;
        float zPosition = (faceReferenceWidth / currentFaceWidth) * referenceDistance;

        // Setting up the postion as the middle of the face
        facePosition = new Vector3(
            mainFace.X + mainFace.Width / 2,
            mainFace.Y + mainFace.Height / 2,
            zPosition
        );

        Debug.Log($"Face position: X={facePosition.x}, Y={facePosition.y}, Z={facePosition.z}");

        CvInvoke.Imshow("Face Detection", source);
    }

    public Vector3 GetFacePosition()
    {
        return facePosition;
    }

    void Update()
    {
        if (video.IsOpened)
        {
            video.Grab();
        }
    }
}

