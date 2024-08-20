using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
	public bool isVeryFastoForward = false;

	[Header("GameFlow")]
	[SerializeField]
	float endGameTimeInSec = 960f;
	private bool _isGameRunning = true;
	[SerializeField]
	float debugTime = 0f;
	private bool _isInFinalCutscene = false;
	[SerializeField]
	GameObject[] blackholes;
	int _currentBlackholeStage = 0;

	[Header("EndGame Stuff")]
	[SerializeField]
	Transform lobbyToShake;
	[SerializeField]
	float startOfBlackhole = 720f;
	[SerializeField]
	public AnimationCurve curve;
	[SerializeField]
	GameObject fadeinUI;
	[SerializeField]
	ModelTable model;
	[SerializeField]
	GameObject fireSource;
	[SerializeField]
	GameObject fadeoutWin;
	[SerializeField]
	GameObject fadeoutLose;
	


	private void Awake()
	{
		if (instance == null && instance != this)
        {
            instance = this;
        }
		debugTime = 0f;
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
        if (Input.GetKeyDown(KeyCode.Q))
		{
			TriggerEarlyEndgame();
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
            else if (isVeryFastoForward)
            {
				_additionnalTimeFromFastForward += (deltaTime * (veryFastForwardMultiplier - 1f));
            }
        }

		if (GetCurrentTime() > 430 && _currentBlackholeStage == 0)
		{
			blackholes[0].SetActive(false);
			blackholes[1].SetActive(true);
			_currentBlackholeStage++;
		}
		else if (GetCurrentTime() > 720f && _currentBlackholeStage == 1)
		{
			blackholes[1].SetActive(false);
			blackholes[2].SetActive(true);
			_currentBlackholeStage++;
		}



		if (_isGameRunning)
		{
			if (GetCurrentTime() > endGameTimeInSec)
			{
				TriggerEndGame();
			}
			_isGameRunning = false;
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
		isVeryFastoForward = false;
	}

	public void StartFastForward()
	{
		_isFastForward = true;
		isVeryFastoForward = false;
	}

	public void StartVeryFastForward()
	{
		_isFastForward = false;
		isVeryFastoForward = true;
	}

	public float GetCurrentTime()
	{
		return _currentIngameTimeBeforeFastForward + _additionnalTimeFromFastForward + debugTime;
	}

	public float GetRemainingTime()
	{
		float remaining = endGameTimeInSec - GetCurrentTime();
		if (remaining < 0f) return 0f;
		return remaining;
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
	}

	public void OnLeaveModelButtonClick()
	{
		GoToLobby();
	}

	public void OnWindowClick()
	{
		Debug.Log("click");
		GoToWindow();
	}

	public void OnModelClick()
	{
		GoToModel();
	}

	public float GetEndGameBlackholeLerp()
	{
		float currentTimeInEndgame = GetCurrentTime() - startOfBlackhole;
		if (currentTimeInEndgame < 0f) return 1f;

		float lerp = (180f - currentTimeInEndgame) / 180f;
		
		if(lerp < 0f) return 0f;

		return lerp;
	}

	public void TriggerEarlyEndgame()
	{
		_isInFinalCutscene = true;
		StartCoroutine(CheckForVictory());
		StartCoroutine(ScreenShake());

	}
	IEnumerator CheckForVictory()
	{
		ModelViewObjs[1].gameObject.SetActive(true);
		bool hasWon = model.CheckIfVictory();
		yield return new WaitForSeconds(10);
		fadeinUI.SetActive(true);
		if(hasWon)
		{
			fadeoutWin.SetActive(true);
		}
		else
		{
			fadeoutLose.SetActive(true);
		}

		yield return new WaitForSeconds(5);
		if(hasWon)
		{
			SceneManager.LoadScene("Scenes/EndWin");
		}
		else
		{
			SceneManager.LoadScene("Scenes/EndLose");

		}
	}
	IEnumerator ScreenShake()
	{
		Vector3 startPos = lobbyToShake.position;
		float elapsedTime = 0f;
		fireSource.gameObject.SetActive(true);

		while (true)
		{
			elapsedTime += Time.deltaTime/100f;
			float str = curve.Evaluate(elapsedTime);
			lobbyToShake.position = startPos + Random.insideUnitSphere * str;
			yield return null;
		}
	}

	private void TriggerEndGame()
	{

	}
}
