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

    void Update()
    {
        float lerpValue = GameManager.instance.GetEndGameBlackholeLerp();
        Vector3 currentPos = Vector3.Lerp(endPos.position, startPos.position, lerpValue);
        blackholePos.position = currentPos;
    }
}
