using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	// Created by Jarryd Worland
	// Last Updated 28/05/2021
	
	//private bool _isPlaying = true;

	public void PauseGame()
	{
		//_isPlaying = !_isPlaying;

		Time.timeScale = 0;
		FindObjectOfType<PauseMenu>(true).gameObject.SetActive(true);
	}

	public void QuitGameplay()
	{
		ResumeGame();
		SceneManager.LoadScene(0);
	}

	public void ResumeGame()
	{
		//_isPlaying = !_isPlaying;

		Time.timeScale = 1;
		FindObjectOfType<PauseMenu>(false).gameObject.SetActive(false);
	}
}