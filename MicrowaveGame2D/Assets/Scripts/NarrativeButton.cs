using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NarrativeButton : MonoBehaviour
{
    public ChangeText narrativeTxt;
    [SerializeField] private Text btnTxt;

    // Start is called before the first frame update
    void Start()
    {
        btnTxt.GetComponent<Text>().text = "Continue";
        Time.timeScale = 0;
    }

    public void SwapText()
    {
        if (btnTxt.GetComponent<Text>().text == "Continue")
        {
            narrativeTxt.SwapText();
            btnTxt.GetComponent<Text>().text = "Begin";
            return;
        }
        else if (btnTxt.GetComponent<Text>().text == "Begin")
        {
            Time.timeScale = 1;
            transform.parent.gameObject.SetActive(false);
        }
    }
}
