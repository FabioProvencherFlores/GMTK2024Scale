using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrbitSlider : MonoBehaviour
{
    public Slider slider;
    public ModelTable table;
	public bool isRadius = true;

	private void Start()
	{
		slider = GetComponent<Slider>();
	}
	void Update()
    {
        if (isRadius)
        {
			table.positionSlider = ((float)slider.value) / 10f;
        }
		else
		{
			table.selectedSpeed = 9 - (int)slider.value;

		}

	}
}
