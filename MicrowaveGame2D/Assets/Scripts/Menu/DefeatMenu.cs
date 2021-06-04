using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
	public class DefeatMenu : MonoBehaviour
	{
		public void Show()
		{
			Time.timeScale = 0;
			FindObjectOfType<DefeatMenu>(true).gameObject.SetActive(true);
		}

		public void QuitGameplay()
		{
			Time.timeScale = 1;
			SceneManager.LoadScene(0);
		}
	}
}