using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSting : MonoBehaviour
{
    // Created by Jarryd Worland
    // Updated 2/06/2021
    void Start()
    {
        
    }

    private static OpenSting instance = null;
    public static OpenSting openSting
    {
        get { return instance; }
    }
    void Awake()
    {

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
