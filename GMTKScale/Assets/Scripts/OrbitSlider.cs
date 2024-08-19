using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbitSlider : MonoBehaviour
{
    public Slider slider;
    public ModelTable table;

	private void Start()
	{
		slider = GetComponent<Slider>();
	}
	void Update()
    {
		table.uiSlider = slider.value;

	}
}
