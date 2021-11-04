using Scripts.Audio;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Scenes
{
	public class SceneFaderBehaviour : MonoBehaviour
	{
		private Image _image;
		private Lerped<Color> _currentColor;

		private bool _isFadingOut;
		private string _targetScene;
		public AudioClip SceneTrans;

		/// <summary>
		/// Get the instance of the scene fader behaviour from the current scene.
		/// </summary>
		public static SceneFaderBehaviour Instance =>
			GameObject.Find("MenuSceneFader").GetComponent<SceneFaderBehaviour>();

		private void Start()
		{
			_image = GetComponent<Image>();
			_currentColor = new Lerped<Color>(Color.black, 0.5f, Easing.EaseIn, true);

			FadeIn();
		}

		private void Update()
		{
			_image.color = _currentColor.Value;

			if (_isFadingOut && _currentColor.Interpolation == 1.0f)
				UnityEngine.SceneManagement.SceneManager.LoadScene(_targetScene);
		}

		/// <summary>
		/// Fade into black and then loads the specified scene.
		/// </summary>
		/// <param name="name"></param>
		public void FadeInto(string name)
		{
			_targetScene = name;
			AudioManager.Play(SceneTrans, AudioCategory.Effect);
			FadeOut();
		}

		private void FadeIn()
		{
			_currentColor.Value = new Color(0.0f, 0.0f, 0.0f, 0.0f);
		}

		private void FadeOut()
		{
			if (_isFadingOut) return;
			_isFadingOut = true;

			_currentColor.Value = new Color(0.0f, 0.0f, 0.0f, 1.0f);
			_image.raycastTarget = true;
		}
	}
}
