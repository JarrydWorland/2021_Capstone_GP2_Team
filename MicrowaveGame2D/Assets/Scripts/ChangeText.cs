using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public string[] passages;
    private int index = 0;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = passages[index];
    }

    public void SwapText()
    {
        index++;
        GetComponent<Text>().text = passages[index];
    }
}
