using System;
using Scripts.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuControlsBehaviour : MenuBehaviour
	{
		private GameObject _keyboardAndMouseSectionObject;
		private GameObject _xboxSectionObject;
		private Text _modeButtonText;

		public override void OnLeave()
		{
			Configuration.Instance.Save();
			base.OnLeave();
		}

		private void Start()
		{
			_keyboardAndMouseSectionObject = transform.Find("SectionKeyboardAndMouse").gameObject;
			_xboxSectionObject = transform.Find("SectionXbox").gameObject;
			_modeButtonText = transform.Find("ModeButton").GetComponentInChildren<Text>();

			UpdateContent();
		}

		/// <summary>
		/// Sets the control scheme.
		/// Called when the "Mode" button is pressed.
		/// </summary>
		public void OnModeButtonPressed()
		{
			ControlScheme controlScheme = Configuration.Instance.ControlScheme + 1;
			if ((int) controlScheme >= Enum.GetValues(typeof(ControlScheme)).Length) controlScheme = 0;
			Configuration.Instance.ControlScheme = controlScheme;

			UpdateContent();
		}

		/// <summary>
		/// Sets the current menu to the previous menu.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnDoneButtonPressed() => MenuManager.GoBack();

		private void UpdateContent()
		{
			_keyboardAndMouseSectionObject.SetActive(false);
			_xboxSectionObject.SetActive(false);

			switch (Configuration.Instance.ControlScheme)
			{
				case ControlScheme.Xbox:
					_xboxSectionObject.SetActive(true);
					_modeButtonText.text = "Mode: Xbox";
					break;
				default:
					_keyboardAndMouseSectionObject.SetActive(true);
					_modeButtonText.text = "Mode: Keyboard & Mouse";
					break;
			}
		}
	}
}