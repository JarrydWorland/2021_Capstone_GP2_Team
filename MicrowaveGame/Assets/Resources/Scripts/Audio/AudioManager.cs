using UnityEngine;
using System.Collections.Generic;
using Scripts.Utilities;

namespace Scripts.Audio
{
    static class AudioManager
	{
		private static AudioId _idCounter = 0;
		private static List<AudioEntry> _audioEntries= new List<AudioEntry>();
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
		/// <returns>The id of the playing audio.</returns>
		public static AudioId Play(AudioClip audioClip, float volume = 0.75f, bool loop = false, float pitch = 1.0f)
		{
			AudioEntry audioEntry = GetAudioEntry();
			audioEntry.AudioSource.clip = audioClip;
			audioEntry.AudioSource.volume = volume;
			audioEntry.AudioSource.loop = loop;
			audioEntry.AudioSource.pitch = pitch;
			audioEntry.AudioSource.Play();
			Log.Info($"Playing {Log.Orange(audioClip.name)} (volume = {Log.Cyan(volume)}, loop = {Log.Cyan(loop)}) using audio entry {Log.Cyan(_audioEntries.IndexOf(audioEntry))}.", LogCategory.AudioManager);
			return audioEntry.Id;
		}

		/// <summary>
		/// Stops all audio sources playing the provided audio clip.
		/// </summary>
		/// <param name="audioClip">The audio clip to stop.</param>
		public static void Stop(AudioId audioId)
		{
			AudioEntry audioEntry = _audioEntries.Find(entry => entry.Id == audioId);
			if (audioEntry != null)
			{
				audioEntry.AudioSource.Stop();
			}
		}

		private static AudioEntry AddAudioEntry()
		{
			AudioEntry audioEntry = new AudioEntry
			{
				Id = _idCounter++,
				AudioSource = Camera.AddComponent<AudioSource>(),
			};
			_audioEntries.Add(audioEntry);
			Log.Info($"Adding additional audio entry (there are now {Log.Cyan(_audioEntries.Count)} audio entries).", LogCategory.AudioManager);
			return audioEntry;
		}

		private static AudioEntry GetAudioEntry()
		{
			AudioEntry audioEntry = _audioEntries.Find(entry => !entry.AudioSource.isPlaying) ?? AddAudioEntry();
			audioEntry.Id = _idCounter++;
			return audioEntry;
		}

		private class AudioEntry
		{
			public AudioId Id;
			public AudioSource AudioSource;			
		}

		private class AudioManagerDestroyBehaviour : MonoBehaviour
		{
			private void OnDestroy()
			{
				_idCounter = 0;
				_audioEntries.Clear();
			}
		}
	}
}
