using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] windowViewObjs;
    [SerializeField]
    GameObject[] ModelViewObjs;

    int currentView = 0; // 0 = window, 1 = model

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentView == 0)
            {
                foreach (GameObject obj in windowViewObjs)
                {
                    obj.SetActive(false);
                }
				foreach (GameObject obj in ModelViewObjs)
				{
					obj.SetActive(true);
				}
                currentView = 1;
			}
            else if (currentView == 1)
            {
                foreach (GameObject obj in windowViewObjs)
                {
                    obj.SetActive(true);
                }
				foreach (GameObject obj in ModelViewObjs)
				{
					obj.SetActive(false);
				}
                currentView = 0;
            }
        }
    }
}
