using UnityEngine;
using System.Collections.Generic;

namespace Scripts.Utilities
{
	static class AudioManager
	{
		private static GameObject Camera => UnityEngine.Camera.main.gameObject;
		private static List<AudioSource> _audioSources = new List<AudioSource>();

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

		private static AudioSource AddAudioSource()
		{
			AudioSource audioSource = Camera.AddComponent<AudioSource>();
			_audioSources.Add(audioSource);
			return audioSource;
		}

		private static AudioSource GetAudioSource() => _audioSources.Find(source => !source.isPlaying) ?? AddAudioSource();
	}
}
