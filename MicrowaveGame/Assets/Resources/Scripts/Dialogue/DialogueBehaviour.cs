using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Scripts.DialogueUI
{
    public class DialogueBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Text objects where strings are entered into 
        /// </summary>
        private Text _speakerName;
        private Text _contentText;

        /// <summary>
        /// Queue that strings are loaded into.
        /// </summary>
        private Queue<string> _sentences;

        public void Awake()
        {
            _sentences = new Queue<string>();
            _speakerName = GameObject.Find("SpeakerText").GetComponent<Text>();
            _contentText = GameObject.Find("DialogueText").GetComponent<Text>();
        }

        /// <summary>
        /// Sets up the Dialogue object for display
        /// </summary>
        /// <param name="dialogue">Dialogue object with all dialogue strings</param>
        public void StartDialogue(Dialogue dialogue)
        {
            GameObject.Find("Player").GetComponent<PlayerInput>().actions.Disable();
            Time.timeScale = 0.0f;

            _speakerName.text = dialogue.Name;

            _sentences.Clear();

            foreach (string sentence in dialogue.Sentences)
            {
                _sentences.Enqueue(sentence);
            }

            DisplayNextSentence();
        }

        /// <summary>
        /// Advances the queue
        /// </summary>
        public void DisplayNextSentence()
        {
            if (_sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            string sentence = _sentences.Dequeue();
            StopCoroutine("TypeSentence");
            StartCoroutine(TypeSentence(sentence));
        }

        /// <summary>
        /// Types out the sentence one character at a time
        /// </summary>
        /// <param name="sentence">Line of dialogue to be typed out</param>
        /// <returns></returns>
        IEnumerator TypeSentence(string sentence)
        {
            _contentText.text = "";

            foreach (char letter in sentence.ToCharArray())
            {
                _contentText.text += letter;
                yield return new WaitForSeconds(0.05f);
            }
        }

        /// <summary>
        /// Ends the dialogue, and de-activates the gameObject
        /// </summary>
        void EndDialogue()
        {
            GameObject.Find("Player").GetComponent<PlayerInput>().actions.Enable();
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }
    }
}
