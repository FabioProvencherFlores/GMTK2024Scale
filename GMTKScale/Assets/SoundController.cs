using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
	private AudioSource _audioSource;
	[SerializeField]
	AudioSource clickSound;
	private void Awake()
	{
		DontDestroyOnLoad(transform.gameObject);
		_audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (Input.GetKeyUp(KeyCode.Mouse0))
		{
			clickSound.Play();
		}
	}

	public void PlayMusic()
	{
		if (_audioSource.isPlaying) return;
		_audioSource.Play();
	}

	public void StopMusic()
	{
		_audioSource.Stop();
	}
}
