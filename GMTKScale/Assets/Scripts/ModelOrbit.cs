using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelOrbit : MonoBehaviour
{
	private LineRenderer _lineRenderer;

	[SerializeField, Range(0f, 1f)]
	public float sliderValue = 0f;

	[SerializeField, Range(5, 500)]
	private int circleResolution = 100;

	public float horizontalDeformation = 1f;
	public float verticalDeformation = 1f;

	private float radius = 3f;
	public float minOrbitRadius = 3f;
	public float maxOrbitRadius = 6f;

	[SerializeField]
	Color selectedMat;
	[SerializeField]
	Color unselectedMat;

	private void Awake()
	{
		_lineRenderer = GetComponent<LineRenderer>();
	}
	void Start()
	{
		_lineRenderer.materials[0].color = unselectedMat;
		DrawOrbit();
	}

	public float SelectOrbit()
	{
		_lineRenderer.materials[0].color = selectedMat;
		_lineRenderer.positionCount = 0;
		DrawOrbit();
		return sliderValue;
	}

	public void UnselectOrbit()
	{
		_lineRenderer.positionCount = 0;
		_lineRenderer.materials[0].color = unselectedMat;
		DrawOrbit();
	}

	public void DrawOrbit()
	{
		_lineRenderer.positionCount = circleResolution;
		radius = minOrbitRadius + ((maxOrbitRadius - minOrbitRadius) * sliderValue);

		for (int i = 0; i < circleResolution; i++)
		{
			float progress = (float)i / circleResolution;
			float currentRadian = progress * 2f * Mathf.PI;

			float x = gameObject.transform.position.x + (Mathf.Cos(currentRadian) * radius * horizontalDeformation);
			float y = gameObject.transform.position.y + 0.1f;
			float z = gameObject.transform.position.z + (Mathf.Sin(currentRadian) * radius * verticalDeformation);

			Vector3 position = new Vector3(x, y, z);
			_lineRenderer.SetPosition(i, position);
		}
	}
}
