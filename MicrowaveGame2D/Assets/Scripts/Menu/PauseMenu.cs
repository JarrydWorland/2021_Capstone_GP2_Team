using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   // Created by Jarryd Worland
   // Last Updated 28/05/2021

    public void PauseGame()
    {
        Time.timeScale = 0;
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
