using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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

    [Header("Speedometer")]
    [SerializeField]
    Transform laserStartPos;
    [SerializeField]
    Transform laserEndPos;
    private LineRenderer _lineRenderer;
    [SerializeField]
    Color targetedColor;
    [SerializeField]
    Color noTargetColor;
    [SerializeField]
    bool showLaserIfNoTarget = true;
    [SerializeField]
    TextMeshProUGUI speedText;

    void Start()
    {
        Debug.Log("Hello");

        if (orbits.Length == 0)
            Debug.LogError("No Planet In Controller", this);

		debugTime = 0f;
		if (Application.isPlaying)
			StartTimer();

        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
		_lineRenderer.SetPosition(0, laserStartPos.position);
		_lineRenderer.SetPosition(1, laserEndPos.position);
	}

    // Update is called once per frame
    void Update()
    {
        bool laserSeesPlanet = false;
        bool isInGame = Application.isPlaying;

		Vector3 laserTempEndPos = laserEndPos.position;
        int speedidx = 0;
        foreach (Orbit orbit in orbits)
        {
            float t = 0f;
            if (isInGame)
            {
			    //_currentTime += Time.deltaTime;
			    t = Time.time;
            }
            else
            {
                t = debugTime;
            }

            orbit.UpdatePosition(t);

            if (orbit.IsInFrontOfLaser())
            {
                if (!laserSeesPlanet)
                {
                    laserTempEndPos = orbit.LaserEndPos.position;
                    speedidx = orbit.speedIdx;

				}
                laserSeesPlanet = true;
            }
		}


        UpdateChronoTime();

        if (laserSeesPlanet)
        {
		    _lineRenderer.positionCount = 2;
            if (isInGame) _lineRenderer.materials[0].color = targetedColor;
		    _lineRenderer.SetPosition(0, laserStartPos.position);
		    _lineRenderer.SetPosition(1, laserTempEndPos);
            int speed = (10 - speedidx) * 50;
            string speedtext = "";
            if (speed < 100) speedtext += "0";
            speedtext += speed.ToString();
            speedText.text = speedtext;
		}
        else
        {
            if (showLaserIfNoTarget)
            {
			    if (isInGame) _lineRenderer.materials[0].color = noTargetColor;
			    _lineRenderer.SetPosition(0, laserStartPos.position);
			    _lineRenderer.SetPosition(1, laserTempEndPos);
            }
            else
            {
		        _lineRenderer.positionCount = 0;
            }

            speedText.text = "000";
        }
        

	}

    private void UpdateChronoTime()
    {
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
