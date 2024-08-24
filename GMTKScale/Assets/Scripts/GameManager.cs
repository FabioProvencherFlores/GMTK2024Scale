using DigitalRuby.RainMaker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


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

	[SerializeField]
	GameObject rocketsound;

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
	public GameObject PlayLightOn, FastLightOn, VeryFastLightOn;
	[SerializeField]
	Color selectedPlaySpeedColor;
	[SerializeField]
	Color selectedFastSpeedColor;
	[SerializeField]
	Color selectedVFSpeedColor;
	[SerializeField]
	Color unselectedSpeedColor;
	[SerializeField]
	Image playSprite;
	[SerializeField]
	Image fastSprite;
	[SerializeField]
	Image veryfastSprite;

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
	[SerializeField]
	RainScript rain;
	[SerializeField]
	GameObject failPos;
	[SerializeField]
	GameObject failSpeed;
	[SerializeField]
	GameObject timeoutMessage;

	[Header("Validator")]
	[SerializeField]
	GameObject validatorHolder;
	[SerializeField]
	Image expectedImage;
	[SerializeField]
	TextAnimator validatorExplanationText;
	[SerializeField]
	GameObject invalidX;
	[SerializeField]
	float verificationDelay = 20f;
	[SerializeField]
	float incorrectPenalty = 15f;
	[SerializeField]
	float confirmationDelay = 15f;
	[SerializeField]
	float randomDelay = 5f;

	public bool isModelValidated = false;

	enum ValidationSteps
	{
		ReadyToStart,
		CheckingPos,
		PosValidated,
		CheckingSpeed,
		SpeedValidated,
		Done,
		Incorrect,
	}

	ValidationSteps currentValidationStep = ValidationSteps.ReadyToStart;

	float _overrideLerpLose = 999f;
	float _overrideLerpWin = 0f;

	bool _isValidatorRunning = false;
	bool _isValidatorInterrupted = false;
	bool _isValidatorDone = false;

	private void Awake()
	{
		if (instance == null && instance != this)
        {
            instance = this;
        }
		debugTime = 0f;
	}

	private void Start()
	{
		StartNormalTime();
		model.Init();
	}

	void Update()
    {
		if (Application.isEditor)
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
				_isGameRunning = false;
			}
		}
	}

	private void GoToWindow()
	{
		if (_isInFinalCutscene) return;
		if (_isValidatorRunning) InterruptValidator();
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
		playSprite.color = selectedPlaySpeedColor;
		fastSprite.color = unselectedSpeedColor;
		veryfastSprite.color = unselectedSpeedColor;
		PlayLightOn.SetActive(true);
		FastLightOn.SetActive(false);
		VeryFastLightOn.SetActive(false);

		if (_isValidatorRunning) InterruptValidator();
	}

	public void StartFastForward()
	{
		_isFastForward = true;
		isVeryFastoForward = false;
		playSprite.color = unselectedSpeedColor;
		fastSprite.color = selectedFastSpeedColor;
		veryfastSprite.color = unselectedSpeedColor;
		PlayLightOn.SetActive(false);
		FastLightOn.SetActive(true);
		VeryFastLightOn.SetActive(false);

		if (_isValidatorRunning) InterruptValidator();
	}

	public void StartVeryFastForward()
	{
		_isFastForward = false;
		isVeryFastoForward = true;
		playSprite.color = unselectedSpeedColor;
		fastSprite.color = unselectedSpeedColor;
		veryfastSprite.color = selectedVFSpeedColor;

		PlayLightOn.SetActive(false);
		FastLightOn.SetActive(false);
		VeryFastLightOn.SetActive(true);
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
		if (_isInFinalCutscene) return;
		if (_isValidatorRunning) InterruptValidator();
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

	public void StartModelValidator()
	{
		StartVeryFastForward();
		if (!_isValidatorRunning)
		{
			currentValidationStep = ValidationSteps.ReadyToStart;
			_isValidatorRunning = true;
			validatorHolder.SetActive(true);
			StartCoroutine(ValidateModel());
		}
	}
	IEnumerator ValidateModel()
	{
		validatorHolder.SetActive(true);
		float nextStepTime = 0f;
		bool isCorrect = false;
		int currentPos = 0;
		while (!_isValidatorInterrupted && !_isValidatorDone)
		{
			float currentTime = GetCurrentTime();

			if (GetCurrentTime() > nextStepTime)
			{
				if (currentValidationStep == ValidationSteps.ReadyToStart)
				{
					Sprite spriteToShow = model.GetExpectedSprite(currentPos);
					if (spriteToShow != null )
					{
						expectedImage.sprite = spriteToShow;
					}
					validatorExplanationText.SetAndStartAnimation("Checking expected position");
					currentValidationStep = ValidationSteps.CheckingPos;
					isCorrect = model.CheckIfPositionIsCorrect(currentPos);
					nextStepTime = currentTime + verificationDelay + Random.Range(0f, randomDelay);
					if (!isCorrect) nextStepTime += incorrectPenalty;
				}
				else if (currentValidationStep == ValidationSteps.CheckingPos)
				{
					// afficher position valid here
					if (isCorrect)
					{
						validatorExplanationText.textToShow = "Position:  correct  ";
						currentValidationStep = ValidationSteps.PosValidated;
					}
					else
					{
						validatorExplanationText.textToShow = "Position: incorrect ";
						invalidX.SetActive(true);
						currentValidationStep = ValidationSteps.Incorrect;
					}
					validatorExplanationText.StartMorph();
					nextStepTime = currentTime + confirmationDelay;
				}
				else if (currentValidationStep == ValidationSteps.PosValidated)
				{
					// afficher speed valid here
					validatorExplanationText.textToShow = "Checking angular speed";
					validatorExplanationText.StartMorph();
					currentValidationStep = ValidationSteps.CheckingSpeed;
					isCorrect = model.CheckIfSpeedIsCorrect(currentPos);
					nextStepTime = currentTime + verificationDelay + Random.Range(0f, randomDelay);
					if (!isCorrect) nextStepTime += incorrectPenalty;
				}
				else if (currentValidationStep == ValidationSteps.CheckingSpeed)
				{
					// afficher position valid here
					if (isCorrect)
					{
						validatorExplanationText.textToShow = "Speed:  correct  ";
						currentValidationStep = ValidationSteps.SpeedValidated;
					}
					else
					{
						validatorExplanationText.textToShow = "Speed: incorrect ";
						invalidX.SetActive(true);
						currentValidationStep = ValidationSteps.Incorrect;
					}
					validatorExplanationText.StartMorph();
					nextStepTime = currentTime + confirmationDelay;
				}
				else if (currentValidationStep == ValidationSteps.SpeedValidated)
				{
					// if speed is valid, current ++ and start again
					if (currentPos < 5)
					{
						currentPos++;
						currentValidationStep = ValidationSteps.ReadyToStart;
					}
					else
					{
						validatorExplanationText.textToShow = "Replica is ready";
						validatorExplanationText.StartMorph();
						nextStepTime = currentTime + 20f;
						currentValidationStep = ValidationSteps.Done;
						isModelValidated = true;
					}
				}
				else if (currentValidationStep == ValidationSteps.Incorrect)
				{
					validatorExplanationText.textToShow = "Please Try again ";
					validatorExplanationText.StartMorph();
					currentValidationStep = ValidationSteps.Done;
					nextStepTime = currentTime + verificationDelay;
				}
				else if (currentValidationStep == ValidationSteps.Done)
				{
					_isValidatorDone = true;
				}
			}


			yield return null;
		}
		StartNormalTime();
		HideValidator();
		validatorExplanationText.textToShow = "";
		_isValidatorRunning = false;
		_isValidatorInterrupted = false;
		_isValidatorDone = false;
	}

	void HideValidator()
	{
		validatorHolder.SetActive(false);
		invalidX.SetActive(false);
	}

	public void InterruptValidator()
	{
		_isValidatorInterrupted = true;
	}

	public bool IsValidatorRunning()
	{
		return _isValidatorRunning;
	}

	public float GetEndGameBlackholeLerp()
	{
		//if (_overrideLerp >= 0f) return _overrideLerp;

		float currentTimeInEndgame = GetCurrentTime() - startOfBlackhole;

		float lerp = (180f - currentTimeInEndgame) / 180f;
		lerp += _overrideLerpWin;
		if (lerp > 1f) lerp = 1f;
		if(lerp < 0f) lerp = 0f;
		if (_overrideLerpLose < lerp) lerp = _overrideLerpLose;

		return lerp;
	}

	public void TriggerEarlyEndgame()
	{
		_isInFinalCutscene = true;

		StartCoroutine(CheckForVictory());
		StartCoroutine(ScreenShake());
		rocketsound.SetActive(true);
	}
	IEnumerator CheckForVictory()
	{
		ModelViewObjs[1].gameObject.SetActive(true);
		ModelViewObjs[2].gameObject.SetActive(false);
		bool positionCorrect = model.CheckIfVictoryPosition();
		bool speedCorrect = model.CheckIfVictorySpeed();
		yield return new WaitForSeconds(6);
		fadeinUI.SetActive(true);
		if(positionCorrect&& speedCorrect)
		{
			StartCoroutine(BackoffBlackhole());
			yield return new WaitForSeconds(4); 
			fadeoutWin.SetActive(true);
		}
		else
		{
			StartCoroutine(ForceBalkhole());
			yield return new WaitForSeconds(2); 
			if (!speedCorrect)
			{
				failSpeed.gameObject.SetActive(true);
			}
			if (!positionCorrect)
			{
				failPos.gameObject.SetActive(true);
			}

			yield return new WaitForSeconds(4); 

			fadeoutLose.SetActive(true);
		}

		yield return new WaitForSeconds(5);
		rocketsound.SetActive(false);
		if(positionCorrect&& speedCorrect)
		{
			SceneManager.LoadScene("Scenes/EndWin");
		}
		else
		{
			SceneManager.LoadScene("Scenes/EndLose");
		}
	}
	IEnumerator BackoffBlackhole()
	{
		float newLerp = 0f;
		while (true)
		{
			newLerp += Time.deltaTime / 3f;
			if (newLerp > 1f) newLerp = 1f;
			_overrideLerpWin = newLerp;
			yield return null;
		}
	}
	IEnumerator ForceBalkhole()
	{
		float newLerp = 1f;
		while (true)
		{
			newLerp -= Time.deltaTime / 3f;
			if(newLerp < 0f) newLerp = 0f;
			_overrideLerpLose = newLerp;
			yield return null;
		}
	}



	IEnumerator ScreenShake()
	{
		Vector3 startPos = lobbyToShake.position;
		float elapsedTime = 0f;
		fireSource.gameObject.SetActive(true);

		while (true)
		{
			elapsedTime += Time.deltaTime/30f;
			if (elapsedTime > 3.5f)
			{
				rain.RainIntensity = 3.5f;
			}
			else
			{
				rain.RainIntensity = elapsedTime;
			}
			float str = curve.Evaluate(elapsedTime);
			if (elapsedTime > 2f)
			{
				str = 2f;
			}

			lobbyToShake.position = startPos + Random.insideUnitSphere * str;
			yield return null;
		}
	}

	private void TriggerEndGame()
	{
		StartCoroutine(TriggerTimeout());
	}

	IEnumerator TriggerTimeout()
	{
		fadeoutLose.SetActive(true);
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene("Scenes/EndLose");
	}
}
