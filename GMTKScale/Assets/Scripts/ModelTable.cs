using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[ExecuteInEditMode]
public class ModelTable : MonoBehaviour
{
    LineRenderer lineRenderer;
    public int circleSteps = 100;
    public float radius = 3f;
    [SerializeField, Range(0f, 1f)]
    public float sliderValue = 0f;

    //[SerializeField]


    [SerializeField] float minOrbitRadius = 0f;
    [SerializeField] float maxOrbitRadius = 100f;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //DrawOrbit();
    }

	private void Update()
	{
        DrawOrbit();
	}

	void DrawOrbit()
    {
        lineRenderer.positionCount = circleSteps;
		radius = minOrbitRadius + ((maxOrbitRadius - minOrbitRadius) * sliderValue);

		for (int i = 0; i < circleSteps; i++)
        {
            float progress = (float)i / circleSteps;
            float currentRadian = progress * 2f * Mathf.PI;

            float x = gameObject.transform.position.x + (Mathf.Cos(currentRadian) * radius);
            float y = gameObject.transform.position.y + 0.1f;
			float z = gameObject.transform.position.z + (Mathf.Sin(currentRadian) * radius);

            Vector3 position = new Vector3(x, y, z);
            lineRenderer.SetPosition(i, position);
        }
	}
}
