using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviour
{

    [SerializeField]
    Transform startPos;
    [SerializeField]
    Transform endPos;
    [SerializeField]
    Transform blackholePos;

    [Header("Countdown")]
    [SerializeField]
    TextAnimator countdownText;
    int _displayedTime = 0;
    [SerializeField]
    TextAnimator warningText;
    float _timeWarninigStarted;
    bool _warningdone = false;
    bool _gameIsDone = false;



	void Update()
    {

        float lerpValue = GameManager.instance.GetEndGameBlackholeLerp();
        Vector3 currentPos = Vector3.Lerp(endPos.position, startPos.position, lerpValue);
        blackholePos.position = currentPos;

        if (_gameIsDone) return;
        if(_warningdone )
        {
            if (GameManager.instance.GetCurrentTime() - _timeWarninigStarted > 10f)
            {
                warningText.gameObject.SetActive(false);
                countdownText.gameObject.SetActive(true);
                _warningdone = false;
                _timeWarninigStarted = 0f;
            }
            return;
        }

        int remainingTime = (int)GameManager.instance.GetRemainingTime();
        if (remainingTime != _displayedTime)
        {
            string timeToShow = "";
            if (remainingTime < 60)
            {
                timeToShow += "0";
            }
            else
            {
                int minutes = (int)remainingTime / 60;
                timeToShow += minutes.ToString();
            }
            timeToShow += ":";
            int seconds = remainingTime % 60;
            if (seconds < 10)
            {
                timeToShow += "0";
            }
            timeToShow += seconds.ToString();

            countdownText.textToShow = timeToShow;
            if (GameManager.instance.isVeryFastoForward)
            {
                countdownText.StartAnimation();
                countdownText.ForceFullText();
            }
            else
            {
                countdownText.StartMorph();
            }

            _displayedTime = remainingTime;
        }
    }

    public void SetWarning()
    {
        if (_warningdone)
        {
			warningText.textToShow = "Launching Shuttle: Attempting Gravity Slingshot Maneuver based on replica";
			warningText.StartMorph();
			GameManager.instance.TriggerEarlyEndgame();
            _gameIsDone = true;
            return;
        }
        else
        {
            _warningdone = true;
            _timeWarninigStarted = GameManager.instance.GetCurrentTime();
            countdownText.gameObject.SetActive(false);
            warningText.gameObject.SetActive(true);
            warningText.StartAnimation();
		}
    }
}
