using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Orbit : MonoBehaviour
{
	private GameObject _gameObject;
	private float _currentLerpValue = 0f;

	[SerializeField]
	[Min(0.001f)]
	private float secondsPerRotation = 1f;

	[SerializeField]
	[Min(1f)]
	public float orbitRadius = 1f;

	private void Awake()
	{
		_gameObject = gameObject;
	}

	//private void Update()
	//{
	//	UpdateOrbit(Time.deltaTime);
	//}

	public void UpdatePosition(float aTime)
	{
		float x = orbitRadius * Mathf.Cos(aTime* 6.283f / secondsPerRotation);
		float z = orbitRadius * Mathf.Sin(aTime* 6.283f / secondsPerRotation);
		float y = 0f;


		gameObject.transform.position = new Vector3(x, y, z);
	}

}
