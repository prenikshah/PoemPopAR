// Copyright (c) 2016 Yasuhide Okamoto
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;

/// <summary>
/// This is a class to rotate a GameObject by using a gyroscope and a compass 
/// to match the direction to the actual azimuth angle. 
/// </summary>
public class RotateWithCompass : MonoBehaviour
{
    double lastCompassUpdateTime = 0;
    Quaternion correction = Quaternion.identity;
    Quaternion targetCorrection = Quaternion.identity;
    /// <summary>
    /// A property to return Input.compass.rawVector, 
    /// On Android, the element changes according to the device orientation, 
    /// so this corrects that change and returns. 
    /// </summary>
    static Vector3 compassRawVector
    {
        get
        {
            Vector3 ret = Input.compass.rawVector;

            if (Application.platform == RuntimePlatform.Android)
            {
                switch (Screen.orientation)
                {
                    case ScreenOrientation.LandscapeLeft:
                        ret = new Vector3(-ret.y, ret.x, ret.z);
                        break;
                    case ScreenOrientation.LandscapeRight:
                        ret = new Vector3(ret.y, -ret.x, ret.z);
                        break;
                    case ScreenOrientation.PortraitUpsideDown:
                        ret = new Vector3(-ret.x, -ret.y, ret.z);
                        break;
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// This is a function to judge a Quaternion includes NaN or infinity values or not. 
    /// </summary>
    static bool isNaN(Quaternion q)
    {
        bool ret = float.IsNaN(q.x) || float.IsNaN(q.y) || float.IsNaN(q.z) || float.IsNaN(q.w) ||
        float.IsInfinity(q.x) || float.IsInfinity(q.y) || float.IsInfinity(q.z) || float.IsInfinity(q.w);

        return ret;
    }

    /// <summary>
    /// This is a function to change the rotation axis of a Quaternion. 
    /// </summary>
    static Quaternion changeAxis(Quaternion q)
    {
        return new Quaternion(-q.x, -q.y, q.z, q.w);
    }

    void Start()
    {
        Input.gyro.enabled = true;
        Input.compass.enabled = true;
    }

    void Update()
    {
        //Quaternion gorientation = changeAxis(Input.gyro.attitude);

        //if (Input.compass.timestamp > lastCompassUpdateTime)
        //{
        //    lastCompassUpdateTime = Input.compass.timestamp;

        //    Vector3 gravity = Input.gyro.gravity.normalized;
        //    Vector3 rawvector = compassRawVector;
        //    Vector3 flatnorth = rawvector - Vector3.Dot(gravity, rawvector) * gravity;

        //    Quaternion corientation = changeAxis(Quaternion.Inverse(Quaternion.LookRotation(flatnorth, -gravity)));

        //    Quaternion tcorrection = corientation * Quaternion.Inverse(gorientation) * Quaternion.Euler(0, 0, 180);
        //    if (!isNaN(tcorrection))
        //    {
        //        targetCorrection = tcorrection;
        //    }
        //}

        //if (Quaternion.Angle(correction, targetCorrection) < 45)
        //    correction = Quaternion.Slerp(correction, targetCorrection, 0.02f);
        //else
        //    correction = targetCorrection;

        //transform.localRotation = correction * gorientation;

        NewStableRotation();


        //Details.text = "Camera transform : " + transform.localRotation + "\n Magnetic Heading :" + Input.compass.magneticHeading + "\n gorientation" + Input.gyro.attitude +
        //               "\n correction Transform: " + correction + "\n Input Gyro" + Input.gyro.attitude;
        //Debug.Log(transform.localRotation);
        //StartCoroutine(TempData());
    }
    void NewStableRotation()
    {
        Quaternion gorientation = changeAxis(Input.gyro.attitude);

        if (Input.compass.timestamp > lastCompassUpdateTime)
        {
            lastCompassUpdateTime = Input.compass.timestamp;

            Vector3 gravity = Input.gyro.gravity.normalized;
            Vector3 rawvector = compassRawVector;
            Vector3 flatnorth = rawvector - Vector3.Dot(gravity, rawvector) * gravity;

            Quaternion corientation = changeAxis(Quaternion.Inverse(Quaternion.LookRotation(flatnorth, -gravity)));

            Quaternion tcorrection = corientation * Quaternion.Inverse(gorientation) * Quaternion.Euler(0, 0, 180);
            if (!isNaN(tcorrection))
            {
                targetCorrection = tcorrection;
            }
        }

        if (Quaternion.Angle(correction, targetCorrection) < 45)
            correction = Quaternion.Slerp(correction, targetCorrection, 0.02f);
        else
            correction = targetCorrection;

        Quaternion e = correction * gorientation;
        transform.eulerAngles = e.eulerAngles;
    }
}
