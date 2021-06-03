using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

//Created by Jarryd Worland
//Last Updated: 2/06/2021

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject OpenSting;

    // Menu Interactions
    public void StartGame()
    {
        OpenSting.SetActive(true);
        SceneManager.LoadScene(1);
    }

    public void QuitGame()
    {
        //if (EditorApplication.isPlaying)
        //   EditorApplication.isPlaying = false;
        //else
            Application.Quit();
    }
}
