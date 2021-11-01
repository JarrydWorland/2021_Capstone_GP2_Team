using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScannerBehaviour : MonoBehaviour
{
    public float CardCount;
    private bool _hubCount;
    private GameObject _counterText;

    // Start is called before the first frame update
    void Start()
    {
        CardCount = Scripts.Persistent.CollectedKeycardCount;
        _hubCount = Scripts.Persistent.FirstTimeInHub;
        if(gameObject.name == "HubRoom_card scanner")
        {
            Canvas canva;
            canva = GameObject.Find("Canvas").GetComponent<Canvas>();
            canva.renderMode = RenderMode.ScreenSpaceOverlay;
            _counterText = new GameObject("counterText");
            _counterText.transform.parent = GameObject.Find("Canvas").transform;
            _counterText.AddComponent<Text>();
            Text sText = _counterText.GetComponent<Text>();

            RectTransform rectTransform;
            rectTransform = sText.GetComponent<RectTransform>();
            rectTransform.localPosition = new Vector3(GameObject.Find("ItemKeyCard").transform.position.x + 1, GameObject.Find("ItemKeyCard").transform.position.y, GameObject.Find("ItemKeyCard").transform.position.z);
            rectTransform.sizeDelta = new Vector2(650, 450);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_hubCount != true)CardCount = Scripts.Persistent.CollectedKeycardCount;
        if (CardCount == 3)
        {
            if (gameObject.name == "Chevron") gameObject.SetActive(true);
            if ((gameObject.name == "ChevronEast") || (gameObject.name == "ChevronWest") || (gameObject.name == "ChevronSouth")) gameObject.SetActive(false);
            if (gameObject.name == "HubRoom_card scanner")
            {
                Text sText = _counterText.GetComponent<Text>();
                sText.font = (Font)Resources.Load("Fonts/Rubik");
                sText.fontSize = 30;
                sText.text = "x" + CardCount;
                sText.alignment = TextAnchor.UpperRight;
            }
        }
        else
        {
            if (gameObject.name == "Chevron") gameObject.SetActive(false);
            if ((gameObject.name == "ChevronEast") || (gameObject.name == "ChevronWest") || (gameObject.name == "ChevronSouth")) gameObject.SetActive(true);
            if (gameObject.name == "HubRoom_card scanner")
            {
                Text sText = _counterText.GetComponent<Text>();
                sText.font = (Font)Resources.Load("Fonts/Rubik");
                sText.fontSize = 30;
                sText.text = "x" + CardCount;
                sText.alignment = TextAnchor.UpperRight;
            }
        }
    }
}
