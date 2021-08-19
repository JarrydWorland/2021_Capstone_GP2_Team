using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.DialogueUI
{
    public class DialogueTrigger : MonoBehaviour
    {
        ///<summary>
        ///Attach this script to anything that we want to trigger an explanation (i.e. First look at an enemy)
        ///Will need to add "using Scripts.DialogueUI" to reference the component
        ///</summary>

        /// <summary>
        /// Dialogue Object
        /// </summary>
        public Dialogue Dialogue;

        /// <summary>
        /// DialogueBehaviour that the Dialogue object is passed into
        /// </summary>
        public DialogueBehaviour DialogueBehaviour;

        /// <summary>
        /// Called when Dialogue needs to be displayed
        /// </summary>
        public void TriggerDialogue()
        {
            DialogueBehaviour.gameObject.SetActive(true);
            DialogueBehaviour.StartDialogue(Dialogue);
        }
    }
}
