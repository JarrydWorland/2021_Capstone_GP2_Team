using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
	public class VictoryMenu : MonoBehaviour
	{
		public void Show()
		{
			Time.timeScale = 0;
			Extensions.FindInActiveObjectByName("VictoryNarrative").SetActive(false);
			FindObjectOfType<VictoryMenu>(true).gameObject.SetActive(true);
		}

		public void QuitGameplay()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(0);
		}
	}
}