using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	// Created by Jarryd Worland
	// Last Updated 28/05/2021
	
	private bool _isPlaying = true;

	public void PauseGame()
	{
		_isPlaying = !_isPlaying;

		Time.timeScale = _isPlaying ? 1 : 0;
		FindObjectOfType<PauseMenu>(true).gameObject.SetActive(_isPlaying);
	}

	public void QuitGameplay()
	{
		ResumeGame();
		SceneManager.LoadScene(0);
	}

	public void ResumeGame()
	{
		Time.timeScale = 1;
	}
}