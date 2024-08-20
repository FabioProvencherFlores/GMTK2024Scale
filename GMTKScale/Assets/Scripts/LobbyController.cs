using System.Collections;
using System.Collections.Generic;
using Unity.Profiling.Editor;
using UnityEngine;

public class LobbyController : MonoBehaviour
{

    [SerializeField]
    Transform startPos;
    [SerializeField]
    Transform endPos;
    [SerializeField]
    Transform blackholePos;

    [SerializeField]
    TextAnimator countdownText;
    int _displayedTime = 0;

    void Update()
    {
        float lerpValue = GameManager.instance.GetEndGameBlackholeLerp();
        Vector3 currentPos = Vector3.Lerp(endPos.position, startPos.position, lerpValue);
        blackholePos.position = currentPos;


        int remainingTime = (int)GameManager.instance.GetRemainingTime();
        if (remainingTime != _displayedTime )
        {
            string timeToShow = "";
            if (remainingTime < 60)
            {
                timeToShow += "0";
            }
            else
            {
                int minutes = (int)remainingTime/60;
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
}
