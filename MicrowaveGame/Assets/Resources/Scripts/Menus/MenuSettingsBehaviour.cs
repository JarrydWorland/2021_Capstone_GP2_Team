using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuSettingsBehaviour : MenuBehaviour
	{
		/// <summary>
		/// Sets the current menu to the "Controls" menu.
		/// Called when the "Controls" button is pressed.
		/// </summary>
		public void OnControlsButtonPressed() => MenuManager.GoInto("MenuControls");

		/// <summary>
		/// Sets the current menu to the "Sounds" menu.
		/// Called when the "Sounds" button is pressed.
		/// </summary>
		public void OnSoundsButtonPressed() => MenuManager.GoInto("MenuSounds");

		/// <summary>
		/// Sets the current menu to the previous menu.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnDoneButtonPressed() => MenuManager.GoBack();

		public void OnControllerTypePressed(Text controllerType)
		{
			if(!PlayerPrefs.HasKey("ControllerType"))
			{
				PlayerPrefs.SetString("ControllerType", "Keyboard");
				controllerType.text = "Controller Type: Keyboard";
			}

			if(PlayerPrefs.GetString("ControllerType") == "Keyboard")
			{
				PlayerPrefs.SetString("ControllerType", "Xbox");
				controllerType.text = "Controller Type: Xbox";
			}
			else if(PlayerPrefs.GetString("ControllerType") == "Xbox")
			{
				PlayerPrefs.SetString("ControllerType", "Keyboard");
				controllerType.text = "Controller Type: Keyboard";
			}

			Debug.Log("LACHLAN: " + PlayerPrefs.GetString("ControllerType"));
		}
	}
}