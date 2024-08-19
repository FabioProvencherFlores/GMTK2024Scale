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
		GoToWindow();
	}

	public void OnModelClick()
	{
		GoToModel();
	}
}
