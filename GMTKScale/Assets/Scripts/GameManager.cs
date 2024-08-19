using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField]
    GameObject[] windowViewObjs;
    [SerializeField]
    GameObject[] ModelViewObjs;
    [SerializeField]
    GameObject[] lobbyViewObjs;

    [SerializeField]
    OrbitSimulatorController windowController;

    [SerializeField]
    public float[] speeds;

    int currentView = 0; // 0 = lobby, 1 = window, 2= model

	[Header("Fast Forward")]
	[SerializeField, Min(1f)]
	float fastForwardMultiplier = 1f;
	[SerializeField, Min(1f)]
	float veryFastForwardMultiplier = 1f;
	private float _additionnalTimeFromFastForward = 0f;
	private float _currentIngameTimeBeforeFastForward = 0f;
	private float _lastTimeUpdate = 0f;
	private bool _isFastForward = false;
	private bool _isVeryFastoForward = false;

	private void Awake()
	{
		if (instance == null && instance != this)
        {
            instance = this;
        }
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
			if (currentView == 0)
			{
				GoToWindow();
			}
			else if (currentView == 1)
			{
				GoToModel();
			}
			else if (currentView == 2)
			{
				GoToLobby();
			}
        }
		if (currentView == 1)
		{
			if (Input.GetKeyDown(KeyCode.Mouse0))
			{
				GoToLobby();
			}

		}

		float engineTime = Time.timeSinceLevelLoad;
		if (_lastTimeUpdate < engineTime)
		{
			float deltaTime = engineTime - _lastTimeUpdate;
			_lastTimeUpdate = engineTime;
			_currentIngameTimeBeforeFastForward += deltaTime;

			if (_isFastForward)
			{
				_additionnalTimeFromFastForward += (deltaTime * (fastForwardMultiplier - 1f));
			}
            else if (_isVeryFastoForward)
            {
				_additionnalTimeFromFastForward += (deltaTime * (veryFastForwardMultiplier - 1f));
            }
        }
	}

	private void GoToWindow()
	{
		foreach (GameObject obj in windowViewObjs)
		{
			obj.SetActive(true);
		}
		foreach (GameObject obj in ModelViewObjs)
		{
			obj.SetActive(false);
		}
		foreach (GameObject obj in lobbyViewObjs)
		{
			obj.SetActive(false);
		}
		windowController.StartTimer();
		currentView = 1;
	}

	public void StartNormalTime()
	{
		_isFastForward = false;
		_isVeryFastoForward = false;
	}

	public void StartFastForward()
	{
		_isFastForward = true;
		_isVeryFastoForward = false;
	}

	public void StartVeryFastForward()
	{
		_isFastForward = false;
		_isVeryFastoForward = true;
	}

	public float GetCurrentTime()
	{
		return _currentIngameTimeBeforeFastForward + _additionnalTimeFromFastForward;
	}

	private void GoToModel()
	{
		foreach (GameObject obj in windowViewObjs)
		{
			obj.SetActive(false);
		}
		foreach (GameObject obj in ModelViewObjs)
		{
			obj.SetActive(true);
		}
		foreach (GameObject obj in lobbyViewObjs)
		{
			obj.SetActive(false);
		}
		currentView = 2;
		StartNormalTime();
	}

	private void GoToLobby()
	{
		foreach (GameObject obj in windowViewObjs)
		{
			obj.SetActive(false);
		}
		foreach (GameObject obj in ModelViewObjs)
		{
			obj.SetActive(false);
		}
		foreach (GameObject obj in lobbyViewObjs)
		{
			obj.SetActive(true);
		}
		currentView = 0;
		StartNormalTime();
	}

	public void OnLeaveModelButtonClick()
	{
		GoToLobby();
	}

	public void OnWindowClick()
	{
		GoToWindow();
	}

	public void OnModelClick()
	{
		GoToModel();
	}
}
