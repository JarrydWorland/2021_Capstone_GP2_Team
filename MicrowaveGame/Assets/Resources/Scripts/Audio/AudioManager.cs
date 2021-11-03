using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Scripts.Utilities;

namespace Scripts.Audio
{
	public static class AudioManager
	{
		private static AudioId _idCounter = 0;
		private static readonly List<AudioEntry> AudioEntries = new List<AudioEntry>();

		private static float[] _audioCategoryVolumes;

		private static float[] AudioCategoryVolumes
		{
			get
			{
				if (_audioCategoryVolumes == null)
				{
					_audioCategoryVolumes = new float[Enum.GetValues(typeof(AudioCategory)).Length];
					for (int i = 0; i < _audioCategoryVolumes.Length; i++) _audioCategoryVolumes[i] = 1.0f;
				}

				return _audioCategoryVolumes;
			}
		}

		private static GameObject _camera;

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
		/// Play a provided audio clip. An audio source will be automatically selected.
		/// </summary>
		/// <param name="audioClip">The audio clip to play.</param>
		/// <param name="category">The category of the audio clip. See <see cref="AudioCategory"/> for available categories.</param>
		/// <param name="volume">The volume the audio clip should be played at.</param>
		/// <param name="loop">Whether or not the the audio clip loop.</param>
		/// <param name="pitch">The pitch the audio clip should be played at.</param>
		/// <returns>The id of the playing audio.</returns>
		public static AudioId Play(AudioClip audioClip, AudioCategory category, float volume = 0.75f, bool loop = false,
			float pitch = 1.0f)
		{
			AudioEntry audioEntry = GetAudioEntry(category);

			audioEntry.AudioSource.clip = audioClip;
			audioEntry.AudioSource.volume = volume * AudioCategoryVolumes[(int) category];
			audioEntry.AudioSource.loop = loop;
			audioEntry.AudioSource.pitch = pitch;
			audioEntry.AudioSource.Play();

			Log.Info($"Playing {Log.Orange(audioClip.name)} (volume = {Log.Cyan(volume)}, loop = {Log.Cyan(loop)}) using audio entry {Log.Cyan(AudioEntries.IndexOf(audioEntry))}.", LogCategory.AudioManager);
			return audioEntry.Id;
		}

		/// <summary>
		/// Stops all audio sources playing the provided audio clip.
		/// </summary>
		/// <param name="audioId">The audio ID to stop.</param>
		public static void Stop(AudioId audioId)
		{
			AudioEntry audioEntry = AudioEntries.Find(entry => entry.Id == audioId);
			if (audioEntry != null) audioEntry.AudioSource.Stop();
		}

		/// <summary>
		/// Gets the playing state for a given audio ID's audio source.
		/// </summary>
		/// <param name="id">The audio ID to match against.</param>
		/// <returns>Returns true if the audio source exists and is playing, otherwise false.</returns>
		public static bool IsPlaying(AudioId id)
		{
			return AudioEntries.FirstOrDefault(entry => entry.Id == id)?.AudioSource.isPlaying ?? false;
		}

		/// <summary>
		/// Gets the given category's volume.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns>The category's volume as a float between 0.0f and 1.0f.</returns>
		public static float GetCategoryVolume(AudioCategory category) => AudioCategoryVolumes[(int) category];

		/// <summary>
		/// Sets the given category's volume, including all audio entries of said category.
		/// </summary>
		/// <param name="category">The category to update.</param>
		/// <param name="volume">The new volume audio entries should be played at.</param>
		public static void SetCategoryVolume(AudioCategory category, float volume)
		{
			AudioCategoryVolumes[(int) category] = volume;

			AudioEntries.FindAll(entry => entry.Category == category)
				.ForEach(entry => entry.AudioSource.volume = volume);
		}

		private static AudioEntry AddAudioEntry()
		{
			AudioEntry audioEntry = new AudioEntry
			{
				AudioSource = Camera.AddComponent<AudioSource>(),
				Id = _idCounter++,
			};

			AudioEntries.Add(audioEntry);

			Log.Info($"Adding additional audio entry (there are now {Log.Cyan(AudioEntries.Count)} audio entries).", LogCategory.AudioManager);
			return audioEntry;
		}

		private static AudioEntry GetAudioEntry(AudioCategory category)
		{
			AudioEntry audioEntry = AudioEntries.Find(entry => !entry.AudioSource.isPlaying) ?? AddAudioEntry();
			audioEntry.Id = _idCounter++;
			audioEntry.Category = category;

			return audioEntry;
		}

		private class AudioEntry
		{
			public AudioId Id;
			public AudioCategory Category;
			public AudioSource AudioSource;
		}

		private class AudioManagerDestroyBehaviour : MonoBehaviour
		{
			private void OnDestroy()
			{
				_idCounter = 0;
				AudioEntries.Clear();
			}
		}
	}
}
