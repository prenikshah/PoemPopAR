// Copyright (c) 2016 Yasuhide Okamoto
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;

/// <summary>
/// This is a class to rotate a GameObject by using only a gyroscope. 
/// </summary>
public class RotateWithGyro : MonoBehaviour {

	void Start () 
	{
		Input.gyro.enabled = true;
	}
	
	void Update () 
	{
		Quaternion gattitude = Input.gyro.attitude;
		gattitude.x *= -1;
		gattitude.y *= -1;
		transform.localRotation = Quaternion.Euler(90, 0, 0) * gattitude;
	}
}
