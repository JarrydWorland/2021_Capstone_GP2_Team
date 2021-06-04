using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public string startText;
    public string replacementText;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = startText;
    }

    public void SwapText()
    {
        GetComponent<Text>().text = replacementText;
    }
}
