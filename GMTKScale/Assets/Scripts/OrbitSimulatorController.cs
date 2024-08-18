using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OrbitSimulatorController : MonoBehaviour
{
    [SerializeField]
    Transform orbitCenter;

    [SerializeField]
    GameObject planetUn;

    [SerializeField]
    Orbit[] orbits;

    [SerializeField]
    [Min(0.001f)]
    float rotationSpeed = 1f;
    [SerializeField]
    float orbitRadius;

    float _currentTime = 0.0f;

    [SerializeField]
    private float debugTime;


    void Start()
    {
        Debug.Log("Hello");

        if (orbits.Length == 0)
            Debug.LogError("No Planet In Controller", this);

		debugTime = 0f;

	}

    // Update is called once per frame
    void Update()
    {
        foreach (Orbit orbit in orbits)
        {
            float t = 0f;
            if (Application.isPlaying)
            {
			    //_currentTime += Time.deltaTime;
			    t = Time.time;
            }
            else
            {
                t = debugTime;
            }

            orbit.UpdatePosition(t);
		}
    }
}
