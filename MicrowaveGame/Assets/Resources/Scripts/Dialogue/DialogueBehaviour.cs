using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBehaviour : MonoBehaviour
{
    /// <summary>
    /// Text objects where strings are entered into 
    /// </summary>
    private Text SpeakerName; 
    private Text ContentText;

    /// <summary>
    /// Queue that strings are loaded into.
    /// </summary>
    private Queue<string> Sentences;

    public void Awake()
    {
        Sentences = new Queue<string>(); // Saw a circularQueue script in Utilities, can switch to that if preferred
        SpeakerName = GameObject.Find("SpeakerText").GetComponent<Text>();
        ContentText = GameObject.Find("DialogueText").GetComponent<Text>();
    }

    /// <summary>
    /// Sets up the Dialogue object for display
    /// </summary>
    /// <param name="dialogue">Dialogue object with all dialogue strings</param>
    public void StartDialogue(Dialogue dialogue)
    {
        Time.timeScale = 0.0f;

        SpeakerName.text = dialogue.Name;

        Sentences.Clear();

        foreach (string sentence in dialogue.Sentences)
        {
            Sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// Advances the queue
    /// </summary>
    public void DisplayNextSentence()
    {
        if (Sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = Sentences.Dequeue();
        StopCoroutine("TypeSentence");
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// Types out the sentence one character at a time
    /// </summary>
    /// <param name="sentence">Line of dialogue to be typed out</param>
    /// <returns></returns>
    IEnumerator TypeSentence (string sentence)
    {
        ContentText.text = "";

        foreach (char letter in sentence.ToCharArray())
        {
            ContentText.text += letter;
            yield return null;
        }
    }

    /// <summary>
    /// Ends the dialogue, and de-activates the gameObject
    /// </summary>
    void EndDialogue()
    {
        Time.timeScale = 1.0f;
        gameObject.SetActive(false);
    }
}
