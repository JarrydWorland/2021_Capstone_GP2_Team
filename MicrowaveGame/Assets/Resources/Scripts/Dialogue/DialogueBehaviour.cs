using System;
using System.Collections.Generic;
using Scripts.Menus;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Dialogue
{
	public class DialogueBehaviour : MonoBehaviour
	{
		private Text _speakerText;
		private Text _contentText;

		private Text _continueButtonText;

		private Queue<string> _sentences;
		private Lerped<string> _currentSentence;
		private int _maxSentenceLength;

		public bool InDialogue { get; private set; }

		public void Awake()
		{
			_speakerText = GameObject.Find("SpeakerText").GetComponent<Text>();
			_contentText = GameObject.Find("DialogueText").GetComponent<Text>();
			_continueButtonText = GameObject.Find("ContinueButton").GetComponentInChildren<Text>();

			_sentences = new Queue<string>();
			_currentSentence = new Lerped<string>(string.Empty, 2.0f, Interpolate, true);
			_maxSentenceLength = int.MinValue;
		}

		private void Update()
		{
			_contentText.text = _currentSentence.Value;
		}

		/// <summary>
		/// Given a dialogue object, load the sentences and display the first sentence.
		/// </summary>
		/// <param name="dialogue"></param>
		public void StartDialogue(Dialogue dialogue)
		{
			InDialogue = true;

			MenuManager.Pause();

			_speakerText.text = dialogue.Speaker;
			_sentences.Clear();

			foreach (string sentence in dialogue.Sentences)
			{
				if (sentence.Length > _maxSentenceLength) _maxSentenceLength = sentence.Length;
				_sentences.Enqueue(sentence);
			}

			DisplayNextSentence();
		}

		/// <summary>
		/// Display the next sentence in the local queue.
		/// </summary>
		public void DisplayNextSentence()
		{
			if (_sentences.Count == 1) _continueButtonText.text = "Begin >>";

			if (_sentences.Count == 0)
			{
				EndDialogue();
				return;
			}

			_currentSentence.Value = _sentences.Dequeue();
		}

		/// <summary>
		/// Disables the dialogue display object and resumes play.
		/// </summary>
		public void EndDialogue()
		{
			gameObject.SetActive(false);
			MenuManager.Resume();

			InDialogue = false;
		}

		/// <summary>
		/// Interpolates between an empty string and the given full string.
		/// Example: "" at 0.0f, "a" at 0.25f ... "abcd" at 1.0f.
		/// </summary>
		private string Interpolate(string _, string line, float interpolation)
		{
			if (string.IsNullOrWhiteSpace(line)) return string.Empty;

			int length = line.Length;
			interpolation = Math.Min(interpolation * _maxSentenceLength / length, 1.0f);

			return line.Substring(0, (int) (length * interpolation));
		}
	}
}