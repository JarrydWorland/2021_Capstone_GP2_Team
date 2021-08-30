using UnityEngine;
using UnityEngine.UI;
using Scripts.Utilities;
using UnityEngine.SceneManagement;

namespace Scripts.Menus
{
	public class MenuNarrativeBehaviour : MenuBehaviour
	{
		/// <summary>
		/// The background music.
		/// </summary>
		public AudioClip BackgroundMusicAudioClip;

		/// <summary>
		/// A debug flag to disable the background music.
		/// </summary>
		public bool DebugDisableBackgroundMusic;

		private readonly string[] _strings =
		{
			"This is the story of Merlin. Merlin was a robot with one purpose: To microwave food. Unfortunately, after an incident involving a small child and a shiny spoon, Merlin was discarded at a scrapyard.",
			"Whilst attempting to escape his situation, Merlin fell through a hole in a pile of scrap, into a hidden world beneath the scrapyard.",
			"Here, we join Merlin as he is being accosted by a group of mechanical soldiers."
		};

		private int _currentString;

		private Text _textObject;
		private Text _buttonTextObject;

		private bool IsFinalString => _currentString == _strings.Length - 1;

		public override void OnEnter()
		{
			base.OnEnter();
			GameState.Pause();
		}

		public override void OnLeave()
		{
			GameState.Resume();
			base.OnLeave();
		}

		private void Start()
		{
			_textObject = transform.Find("Text").GetComponent<Text>();
			_buttonTextObject = transform.Find("Button").GetComponentInChildren<Text>();

			UpdateTexts();

			if (!DebugDisableBackgroundMusic)
			{
				AudioManager.Play(BackgroundMusicAudioClip);
			}

			MenuManager.Init(this);
		}

		/// <summary>
		/// Switches to the "Playing" menu if the final string has been displayed, otherwise updates the text to show
		/// the next string.
		/// Called when the "Continue" / "Begin" button is pressed.
		/// </summary>
		public void OnButtonPressed()
		{
			if (IsFinalString)
			{
				MenuManager.GoInto("MenuPlaying");
			}
			else
			{
				_currentString++;
				UpdateTexts();
			}
		}

		private void UpdateTexts()
		{
			_textObject.text = _strings[_currentString];
			_buttonTextObject.text = IsFinalString ? "Begin" : "Continue";
		}
	}
}
