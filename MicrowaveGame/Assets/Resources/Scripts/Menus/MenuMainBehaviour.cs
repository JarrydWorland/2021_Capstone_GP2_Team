using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scripts.Menus
{
	public class MenuMainBehaviour : MonoBehaviour
	{
		public void OnStartButtonPressed() => SceneManager.LoadScene("Gameplay");

		public void OnControlsButtonPressed()
		{
			gameObject.SetActive(false);
			transform.parent.Find("Controls").gameObject.SetActive(true);
		}

		public void OnCreditsButtonPressed()
		{
			gameObject.SetActive(false);
			transform.parent.Find("Credits").gameObject.SetActive(true);
		}

		public void OnQuitButtonPressed()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
		}
	}
}