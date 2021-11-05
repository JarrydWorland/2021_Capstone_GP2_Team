using Scripts.Audio;
using Scripts.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Scripts.Menus
{
	public class MenuCutsceneBehaviour : MenuBehaviour
	{
		public GameObject VideoObject;

		private void Start()
		{
			MenuManager.Init(this);

			VideoPlayer videoPlayer = VideoObject.GetComponent<VideoPlayer>();
			videoPlayer.loopPointReached += _ => PerformPostVideo();

			videoPlayer.SetDirectAudioVolume(0, AudioManager.GetCategoryVolume(AudioCategory.Music) * 0.4f);
			videoPlayer.SetDirectAudioVolume(1, AudioManager.GetCategoryVolume(AudioCategory.Music) * 0.4f);
			videoPlayer.Play();
		}

		/// <summary>
		/// Skips the cutscene by loading into the hub scene.
		/// Called when the "Skip" button is pressed.
		/// </summary>
		public void OnSkipButtonPressed() => PerformPostVideo();

		private void PerformPostVideo()
		{
			if (SceneManager.GetActiveScene().name == "OpeningCutscene")
				SceneFaderBehaviour.Instance.FadeInto("Hub");
		}
	}
}