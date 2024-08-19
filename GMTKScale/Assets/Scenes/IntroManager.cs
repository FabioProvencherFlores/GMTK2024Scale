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
	}

	public void OnNextSlide()
    {
        StartCoroutine(NextSlide());
    }

	IEnumerator NextSlide()
    {
        fadeoutSquare.SetActive(true);
        yield return new WaitForSeconds(2);

        SceneManager.LoadScene(nextScene);
    }
}
