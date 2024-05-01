// Copyright (c) 2016 Yasuhide Okamoto
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;

/// <summary>
/// This is a class to toggle a rotation method: RotateWithGyro and RotateWithCompass. 
/// </summary>
public class ChangeRotationMethod : MonoBehaviour
{

    public enum RotateMethod
    {
        Gyro,
        Compass,
    }

    [SerializeField]
    RotateMethod rotateMethod = RotateMethod.Gyro;
    public RotateMethod CurrentRotateMethod
    {
        get { return rotateMethod; }
    }

    RotateWithGyro rgyro;
    RotateWithCompass rcompass;

    void SetRotateMethod()
    {
        if (rgyro != null)
            rgyro.enabled = (rotateMethod == RotateMethod.Gyro);
        if (rcompass != null)
            rcompass.enabled = (rotateMethod == RotateMethod.Compass);
    }

    void Start()
    {
        rgyro = GetComponent<RotateWithGyro>();
        rcompass = GetComponent<RotateWithCompass>();

        SetRotateMethod();
        rotateMethod = RotateMethod.Compass;
    }


    void Update1()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (rotateMethod == RotateMethod.Gyro)
            {
                Debug.Log("Compass method");

                rotateMethod = RotateMethod.Compass;
            }
            else
            {
                Debug.Log("Gyro method");

                rotateMethod = RotateMethod.Gyro;
            }


            SetRotateMethod();
        }
    }
}
