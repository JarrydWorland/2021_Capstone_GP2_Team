using UnityEngine;
using UnityEngine.UI;
using Scripts.Utilities;

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

		[SerializeField]
		[TextArea(3, 10)]
		private string[] _strings;

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
			if (!Persistent.FirstTimeInHub) MenuManager.GoInto("MenuPlaying");
			Persistent.FirstTimeInHub = false;
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
