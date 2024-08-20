using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;



public class Orbit : MonoBehaviour
{
	[SerializeField]
	public bool overrideSpeed = false;
	[SerializeField]
	[Min(0.001f)]
	private float secondsPerRotation = 1f;
	[SerializeField]
	float blackholeoffet = 0f;

	[SerializeField, Range(0, 9)]
	public int speedIdx = 0;

	[SerializeField]
	[Min(1f)]
	public float orbitRadius = 1f;

	public float currentAngle = 0f;

	[Header("Laser Angles")]
	[SerializeField, Range(1.5f, 2.1f)]
	float minAngleForLaser = 0f;
	[SerializeField, Range(1.5f, 2.1f)]
	float maxAngleForLaser = 0f;
	[SerializeField]
	bool debugSkip = false;
	[SerializeField]
	public Transform LaserEndPos;

	private void Start()
	{
		if (!overrideSpeed)
		{
			secondsPerRotation = GameManager.instance.speeds[speedIdx];
		}
	}

	public void UpdatePosition(float aTime)
	{
		float processedTime = aTime;
		while (processedTime > secondsPerRotation)
		{
			processedTime -= secondsPerRotation;
		}

		currentAngle = processedTime * 6.28319f / secondsPerRotation;
		currentAngle -= blackholeoffet;
		if (overrideSpeed)
		{
			currentAngle *= -1f;
		}

		float x = orbitRadius * Mathf.Cos(currentAngle);
		float z = orbitRadius * Mathf.Sin(currentAngle);
		float y = 0f;


		gameObject.transform.position = new Vector3(x, y, z);
	}

	public bool IsInFrontOfLaser()
	{
        if (debugSkip) return false;
        if (currentAngle < minAngleForLaser) return false;
		if (currentAngle > maxAngleForLaser) return false;

		return true;
	}
}
