using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    private float timeOpened = 0f;

    [SerializeField]
    private TextMeshProUGUI chrono;


    void Start()
    {
        Debug.Log("Hello");

        if (orbits.Length == 0)
            Debug.LogError("No Planet In Controller", this);

		debugTime = 0f;
		if (Application.isPlaying)
			StartTimer();
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

		//double mainGameTimerd = (double)GetTimeSinceOpened();
		//TimeSpan time = TimeSpan.FromSeconds(mainGameTimerd);
  //      string displayTime = time.Minutes.ToString() + ":" + time.Seconds.ToString() + ":" + time.Milliseconds.ToString(); 

        float timeMS = GetTimeSinceOpened();
        int timeS = (int)timeMS;
        string displayTime = "";
        if (timeS < 100)
            displayTime += "0";
        if (timeS < 10)
            displayTime += "0";
        displayTime += timeS.ToString();
        chrono.text = displayTime;
	}

    public float GetTimeSinceOpened()
    {
        return Time.time - timeOpened;
    }

    public void StartTimer()
    {
        timeOpened = Time.time;
    }
}
