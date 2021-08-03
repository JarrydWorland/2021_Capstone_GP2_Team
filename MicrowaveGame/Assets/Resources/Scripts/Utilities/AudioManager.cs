using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Utilities
{
	static class AudioManager
	{
		private static List<AudioSource> _audioSources = new List<AudioSource>();
		private static GameObject _camera = null;
		private static GameObject Camera
		{
			get
			{
				if (_camera == null)
				{
					_camera = UnityEngine.Camera.main.gameObject;
					_camera.AddComponent<AudioManagerDestroyBehaviour>();
				}
				return _camera;
			}
		}

		/// <summary>
		/// Play a provided audio clip. An audio source will be automatically
		/// selected.
		/// </summary>
		/// <param name="audioClip">The audio clip to play.</param>
		/// <param name="volume">the volume the audio clip should be played at.</param>
		/// <param name="loop">should the audio clip be looped.</param>
		public static void Play(AudioClip audioClip, float volume = 0.75f, bool loop = false)
		{
			AudioSource audioSource = GetAudioSource();
			audioSource.clip = audioClip;
			audioSource.volume = volume;
			audioSource.loop = loop;
			audioSource.Play();
		}

		/// <summary>
		/// Stops all audio sources playing the provided audio clip.
		/// </summary>
		/// <param name="audioClip">The audio clip to stop.</param>
		public static void Stop(AudioClip audioClip = null)
		{
			IEnumerable<AudioSource> sourcesPlayingClip = _audioSources.Where(source => source.clip == audioClip);
			foreach(AudioSource audioSource in sourcesPlayingClip)
			{
				audioSource.Stop();
			}
		}

		private static AudioSource AddAudioSource()
		{
			AudioSource audioSource = Camera.AddComponent<AudioSource>();
			_audioSources.Add(audioSource);
			return audioSource;
		}

		private static AudioSource GetAudioSource() => _audioSources.Find(source => !source.isPlaying) ?? AddAudioSource();

		private class AudioManagerDestroyBehaviour : MonoBehaviour
		{
			private void OnDestroy() => _audioSources.Clear();
		}
	}
}
