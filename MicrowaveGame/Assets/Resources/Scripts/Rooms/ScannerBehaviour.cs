using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScannerBehaviour : MonoBehaviour
{
    public float CardCount;
    private bool _hubCount;

    // Start is called before the first frame update
    void Start()
    {
        CardCount = Scripts.Persistent.CollectedKeycardCount;
        _hubCount = Scripts.Persistent.FirstTimeInHub;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hubCount != true)CardCount = Scripts.Persistent.CollectedKeycardCount;
        if (CardCount == 3)
        {
            if (gameObject.name == "Chevron") gameObject.SetActive(true);
            if ((gameObject.name == "ChevronEast") || (gameObject.name == "ChevronWest") || (gameObject.name == "ChevronSouth")) gameObject.SetActive(false);
        }
        else
        {
            if (gameObject.name == "Chevron") gameObject.SetActive(false);
            if ((gameObject.name == "ChevronEast") || (gameObject.name == "ChevronWest") || (gameObject.name == "ChevronSouth")) gameObject.SetActive(true);
        }
    }
}
