using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeText : MonoBehaviour
{
    public string[] passages;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Text>().text = passages[0];
    }

    public void SwapText()
    {
        if (AtEnd())
        {
            return;
        }

        for (int i = 0; i < passages.Length; i++)
        {
            if (passages[i] == GetComponent<Text>().text)
            {
                GetComponent<Text>().text = passages[i+1];
                break;
            }
        }
        
    }

    public bool AtEnd()
    {
        return (passages[passages.Length - 1] == GetComponent<Text>().text);
    }
}
