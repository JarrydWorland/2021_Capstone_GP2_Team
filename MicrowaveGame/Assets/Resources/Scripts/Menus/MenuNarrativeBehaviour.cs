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

		/*[SerializeField]
		[TextArea(3, 10)]*/
		private string[] _strings =
		{
			"Welcome to L.E.D: Light Extraction Droid demo. You are about to assume the role of Merlin, a robot working as a private detective who serves the citizens of Light City",
			"Merlin has just taken back control of the Light City Power Grid from the villainous Lightbulb Gang, who have stolen the keycards that provide power to Light City",
			"Now, Merlin must infiltrate their place of operations known as the Lightclub, and gain back control of the keycards. It is here that we join Merlin. Good luck,"
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
			if (Persistent.ShownNarritive) MenuManager.GoInto("MenuPlaying");
			Persistent.ShownNarritive = true;
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
