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

	public float positionSlider = 0f;
    private int _currentSelectIdx = 0;
    public int selectedSpeed = 0; // de 0 a 9

    [SerializeField]
    bool _checkPosition = true;
    [SerializeField]
    bool _checkSpeed = true;

    bool _isInit = false;

    void Awake()
    {
        if (modelOrbits.Length == 0)
            Debug.LogError("No Orbits to draw", this);

        if (!_isInit)
        {
			Init();
        }
    }

    public void Init()
    {
		float value = 0.1f;
		int sidx = 0;
		foreach (ModelOrbit modelOrbit in modelOrbits)
		{
			modelOrbit.minOrbitRadius = minOrbitRadius;
			modelOrbit.maxOrbitRadius = maxOrbitRadius;

			modelOrbit.horizontalDeformation = horizontalDeformation;
			modelOrbit.verticalDeformation = verticalDeformation;

			modelOrbit.positionSliderValue = value;
			modelOrbit.selectedSpeediDx = sidx;
			value += 0.1f;
			sidx++;
		}

		_isInit = true;
	}

	private void Start()
	{
		positionSlider = modelOrbits[_currentSelectIdx].SelectOrbit();
	}
	public void SelectNext()
    {
        modelOrbits[_currentSelectIdx].UnselectOrbit();
        Debug.Log("CHANGE");
        _currentSelectIdx++;
        _currentSelectIdx %= modelOrbits.Length;

        positionSlider = modelOrbits[_currentSelectIdx].SelectOrbit();
        speedSlider.value = 9f - (float)modelOrbits[_currentSelectIdx].selectedSpeediDx;
        radiusSlider.value = positionSlider * 10f;
	}

	private void Update()
	{
        if (Application.isEditor)
        {
            if (Input.GetKey(KeyCode.K))
            {
                CheatPositionSolution();
            }
			if (Input.GetKey(KeyCode.L))
			{
				CheatSpeedSolution();
			}
		}


        int idx = 0;
		foreach (ModelOrbit modelOrbit in modelOrbits)
		{
            if (idx == _currentSelectIdx)
            {
                modelOrbit.positionSliderValue = positionSlider;
                modelOrbit.selectedSpeediDx = selectedSpeed;
            }

            modelOrbit.secondsPerRotation = GameManager.instance.speeds[modelOrbit.selectedSpeediDx];
			modelOrbit.horizontalDeformation = horizontalDeformation;
			modelOrbit.verticalDeformation = verticalDeformation;

			idx++;
            modelOrbit.DrawOrbit();
            modelOrbit.UpdatePosition(GameManager.instance.GetCurrentTime());
		}
	}

	public bool CheckIfPositionIsCorrect(int orderPos)
	{
		float lowbound = -1f;

		for (int i = 0; i < 6; i++)
		{
			float smallest = 999f;
			int answer = -1;


			foreach (ModelOrbit orb in modelOrbits)
			{
				if (orb.positionSliderValue < smallest && orb.positionSliderValue > lowbound)
				{
					smallest = orb.positionSliderValue;
					answer = orb.answerPos;
				}
			}

			if (answer != i)
			{
				return false;
			}

			if (i == orderPos)
			{
				return true;
			}

			lowbound = smallest;
		}

		return false;
	}

	public bool CheckIfSpeedIsCorrect(int orderPos)
	{
		int lowbound = -1;

		for (int i = 0; i < 6; i++)
		{
			int smallest = 999;
			int answer = -1;


			foreach (ModelOrbit orb in modelOrbits)
			{
				if (orb.selectedSpeediDx < smallest && orb.selectedSpeediDx > lowbound)
				{
					smallest = orb.selectedSpeediDx;
					answer = orb.answerSpeed;
				}
			}
			if (answer - 1 != i)
			{
				return false;
			}

			if (i == orderPos)
			{
				return true;
			}

			lowbound = smallest;
		}
		return false;
	}

	public Sprite GetExpectedSprite(int orderPos)
    {
		float lowbound = -1f;

		for (int i = 0; i < 6; i++)
		{
			float smallest = 999f;
			Sprite returnSprite = null;


			foreach (ModelOrbit orb in modelOrbits)
			{
				if (orb.positionSliderValue < smallest && orb.positionSliderValue > lowbound)
				{
					smallest = orb.positionSliderValue;
					returnSprite = orb.imageToShow;
				}
			}

			lowbound = smallest;
            if (i == orderPos)
            {
                return returnSprite;
            }
		}


		return null;
	}

    void CheatPositionSolution()
    {
		foreach (ModelOrbit modelOrbit in modelOrbits)
        {
            modelOrbit.UnselectOrbit();
        }

        positionSlider = modelOrbits[0].positionSliderValue = 0.2f;
		positionSlider = modelOrbits[1].positionSliderValue = 0.9f;
		positionSlider = modelOrbits[2].positionSliderValue = 0.8f;
		positionSlider = modelOrbits[3].positionSliderValue = 0.1f;
		positionSlider = modelOrbits[4].positionSliderValue = 0.3f;
		positionSlider = modelOrbits[5].positionSliderValue = 0.5f;

        SelectNext();
	}

    void CheatSpeedSolution()
    {
		foreach (ModelOrbit modelOrbit in modelOrbits)
		{
			modelOrbit.UnselectOrbit();
		}

		positionSlider = modelOrbits[0].selectedSpeediDx = 2;
		positionSlider = modelOrbits[1].selectedSpeediDx = 8;
		positionSlider = modelOrbits[2].selectedSpeediDx = 4;
		positionSlider = modelOrbits[3].selectedSpeediDx = 1;
		positionSlider = modelOrbits[4].selectedSpeediDx = 7;
		positionSlider = modelOrbits[5].selectedSpeediDx = 3;

        SelectNext();
    }

	public bool CheckIfVictoryPosition()
    {
        if ( _checkPosition)
        {
            float lowbound = -1f;

            for (int i = 0; i < 6; i++)
            {
                float smallest = 999f;
                int answer = -1;


                foreach (ModelOrbit orb in modelOrbits)
                {
                    if (orb.positionSliderValue < smallest && orb.positionSliderValue > lowbound)
                    {
                        smallest = orb.positionSliderValue;
                        answer = orb.answerPos;
                    }
                }

                if (answer != i)
                {
                    return false;
                }

                lowbound = smallest;
            
            }
        }

        return true;
    }

	public bool CheckIfVictorySpeed()
    {
		if (_checkSpeed)
		{
			int lowbound = -1;

			for (int i = 0; i < 6; i++)
			{
				int smallest = 999;
				int answer = -1;


				foreach (ModelOrbit orb in modelOrbits)
				{
					if (orb.selectedSpeediDx < smallest && orb.selectedSpeediDx > lowbound)
					{
						smallest = orb.selectedSpeediDx;
						answer = orb.answerSpeed;
					}
				}
                if (answer-1 != i)
				{
					return false;
				}

				lowbound = smallest;

			}
		}

		return true;
	}
}
