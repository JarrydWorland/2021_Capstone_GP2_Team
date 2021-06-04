using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndNarrativeButton : MonoBehaviour
{
    public ChangeText narrativeTxt;
    [SerializeField] private Text btnTxt;

    // Start is called before the first frame update
    void Awake()
    {
        btnTxt.GetComponent<Text>().text = "Continue";
        transform.parent.gameObject.SetActive(true);
    }

    public void SwapText()
    {
        if (btnTxt.GetComponent<Text>().text == "Return to Main Menu")
        {
            SceneManager.LoadScene("MainMenu");
        }
        else
        {
            narrativeTxt.SwapText();
        }

        if (narrativeTxt.AtEnd())
        {
            btnTxt.GetComponent<Text>().text = "Return to Main Menu";
            return;
        } 
    }
}
