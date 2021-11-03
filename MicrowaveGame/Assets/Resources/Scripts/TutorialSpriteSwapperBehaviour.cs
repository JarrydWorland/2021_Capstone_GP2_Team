using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpriteSwapperBehaviour : MonoBehaviour
{
    public GameObject[] xboxControllerIcons;
    public GameObject[] keyboardControllerIcons;

    // Start is called before the first frame update
    void Start()
    {
        if(!PlayerPrefs.HasKey("ControllerType"))
        {
            PlayerPrefs.SetString("ControllerType", "Keyboard");
        }

        if (PlayerPrefs.GetString("ControllerType") == "Keyboard")
        {
            foreach (GameObject g in xboxControllerIcons)
            {
                g.SetActive(false);
            }
            foreach (GameObject g in keyboardControllerIcons)
            {
                g.SetActive(true);
            }
        }
        else if(PlayerPrefs.GetString("ControllerType") == "Xbox")
        {
            foreach (GameObject g in keyboardControllerIcons)
            {
                g.SetActive(false);
            }
            foreach (GameObject g in xboxControllerIcons)
            {
                g.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
