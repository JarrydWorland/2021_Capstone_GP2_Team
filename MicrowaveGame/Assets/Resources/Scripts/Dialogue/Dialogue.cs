using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.DialogueUI
{
    [System.Serializable]
    public class Dialogue
    {
        /// <summary>
        /// Name of the speaker
        /// </summary>
        public string Name;

        /// <summary>
        /// Sentences that the speaker will say
        /// </summary>
        [TextArea(3, 10)]
        public string[] Sentences;
    }
}
