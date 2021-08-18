using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    ///<summary>
    ///Attach this script to anything that we want to trigger an explanation (i.e. First look at an enemy)
    ///</summary>

    /// <summary>
    /// Dialogue Object
    /// </summary>
    public Dialogue dialogue;

    /// <summary>
    /// Called when Dialogue needs to be displayed
    /// </summary>
    public void TriggerDialogue()
    {
        GameObject.Find("Canvas").transform.Find("DialogueDisplay").gameObject.SetActive(true);
        FindObjectOfType<DialogueBehaviour>().StartDialogue(dialogue);
    }
}
