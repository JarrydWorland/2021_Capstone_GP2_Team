using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpriteSwapper : MonoBehaviour
{
    public GameObject[] xboxControllerIcons;
    public GameObject[] keyboardControllerIcons;
    [SerializeField] private bool debugControllerIconSwapped = false;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetString("ControllerType") == "Keyboard" || debugControllerIconSwapped)
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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
