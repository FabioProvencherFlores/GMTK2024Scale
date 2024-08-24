using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    TextAnimator titletext;

    [SerializeField]
    GameObject fadeoutSquare;

    [SerializeField]
    string nextScene;

    [SerializeField]
    bool triggerOnClick = false;
	[SerializeField]
	bool longWait = false;

    [SerializeField]
    bool makeButtonAppear = false;
    [SerializeField]
    GameObject button;

	void Start()
    {
        titletext.StartAnimation();
    }

	private void Update()
	{
        if (triggerOnClick)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (titletext.GetIsAnimating())
                {
                    titletext.ForceFullText();
                }
                else
                {
                    OnNextSlide();
                    triggerOnClick = false;
                    
                }
			}
        }

        if (!titletext.GetIsAnimating() && makeButtonAppear)
        {
            button.SetActive(true);
            makeButtonAppear = false;
        }
	}

	public void OnNextSlide()
    {
		GameObject.FindGameObjectWithTag("Music").GetComponent<SoundController>().PlayMusic();
		StartCoroutine(NextSlide());
    }

    public void RetryGame()
    {
        triggerOnClick = false;
        StartCoroutine(BackToMain());
	}

	IEnumerator BackToMain()
    {
		fadeoutSquare.SetActive(true);
		yield return new WaitForSeconds(2);
		SceneManager.LoadScene("Scenes/Main");
    }

	IEnumerator NextSlide()
    {
        fadeoutSquare.SetActive(true);
        yield return new WaitForSeconds(2);
        if (longWait)
        {
            yield return new WaitForSeconds(5);
        }
        SceneManager.LoadScene(nextScene);
    }
}
