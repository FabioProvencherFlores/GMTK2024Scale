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

    [SerializeField]
    Slider slider;

    public float uiSlider = 0f;
    private int _currentSelectIdx = 0;

    void Awake()
    {
        if (modelOrbits.Length == 0)
            Debug.LogError("No Orbits to draw", this);
        
        foreach ( ModelOrbit modelOrbit in modelOrbits )
        {
            modelOrbit.minOrbitRadius = minOrbitRadius;
            modelOrbit.maxOrbitRadius = maxOrbitRadius;
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
        slider.value = uiSlider;
	}

	private void Update()
	{
        int idx = 0;
		foreach (ModelOrbit modelOrbit in modelOrbits)
		{
            if (idx == _currentSelectIdx)
            {
                modelOrbit.sliderValue = uiSlider;
            }

            idx++;
            modelOrbit.DrawOrbit();
		}
	}
}
