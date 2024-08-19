using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[ExecuteInEditMode]
public class ModelTable : MonoBehaviour
{

    [SerializeField]
    ModelOrbit[] modelOrbits;

    [SerializeField] float minOrbitRadius = 0f;
    [SerializeField] float maxOrbitRadius = 100f;
    
    [SerializeField] float horizontalDeformation = 1f;
    [SerializeField] float verticalDeformation = 1f;

    [SerializeField]
    Slider radiusSlider;

	[SerializeField]
	Slider speedSlider;

	public float uiSlider = 0f;
    private int _currentSelectIdx = 0;
    public int selectedSpeed = 0; // de 0 a 9

    void Awake()
    {
        if (modelOrbits.Length == 0)
            Debug.LogError("No Orbits to draw", this);

        float value = 0f;
        int sidx = 0;
        foreach ( ModelOrbit modelOrbit in modelOrbits )
        {
            modelOrbit.minOrbitRadius = minOrbitRadius;
            modelOrbit.maxOrbitRadius = maxOrbitRadius;
            
            modelOrbit.horizontalDeformation = horizontalDeformation;
            modelOrbit.verticalDeformation = verticalDeformation;

            modelOrbit.sliderValue = value;
            modelOrbit.selectedSpeediDx = sidx;
            value += 0.1f;
			sidx++;

		}
    }

	private void Start()
	{
		uiSlider = modelOrbits[_currentSelectIdx].SelectOrbit();
	}
	public void SelectNext()
    {
        modelOrbits[_currentSelectIdx].UnselectOrbit();
        Debug.Log("CHANGE");
        _currentSelectIdx++;
        _currentSelectIdx %= modelOrbits.Length;

        uiSlider = modelOrbits[_currentSelectIdx].SelectOrbit();
        speedSlider.value = 9f - (float)modelOrbits[_currentSelectIdx].selectedSpeediDx;
        radiusSlider.value = uiSlider * 10f;
	}

	private void Update()
	{
        int idx = 0;
		foreach (ModelOrbit modelOrbit in modelOrbits)
		{
            if (idx == _currentSelectIdx)
            {
                modelOrbit.sliderValue = uiSlider;
                modelOrbit.selectedSpeediDx = selectedSpeed;
            }

            modelOrbit.secondsPerRotation = GameManager.instance.speeds[modelOrbit.selectedSpeediDx];
			modelOrbit.horizontalDeformation = horizontalDeformation;
			modelOrbit.verticalDeformation = verticalDeformation;

			idx++;
            modelOrbit.DrawOrbit();
            modelOrbit.UpdatePosition(Time.time);
		}
	}
}
