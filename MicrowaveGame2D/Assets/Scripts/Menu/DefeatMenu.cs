using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DefeatMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (this.gameObject.activeSelf && Time.timeScale > 0) //stop game when active and don't enter if game is stopped
        {
            Time.timeScale = 0;
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1); //using Jarryd's code
    }

    public void ReturnToMain()
    {
        SceneManager.LoadScene(0); //using Jarryd's code
    }
}
