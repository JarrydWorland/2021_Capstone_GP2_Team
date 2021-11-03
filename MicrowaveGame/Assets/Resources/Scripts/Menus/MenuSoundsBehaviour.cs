using System;
using System.Linq;
using Scripts.Audio;
using UnityEngine;
using UnityEngine.UI;
using Scripts.Config;

namespace Scripts.Menus
{
	public class MenuSoundsBehaviour : MenuBehaviour
	{
		private GameObject _effectVolumeObject, _musicVolumeObject;
		private Text _effectVolumeValueText, _musicVolumeValueText;

		private AudioClip _scrollAudioClip;
		private AudioId _scrollAudioId;

		public override void OnEnter()
		{
			base.OnEnter();

			_scrollAudioClip ??= Resources.Load<AudioClip>("Audio/Effects/UI/Scroll");

			float effectVolume = AudioManager.GetCategoryVolume(AudioCategory.Effect);

			_effectVolumeObject ??= GameObject.Find("EffectVolumeSlider");
			_effectVolumeValueText = _effectVolumeObject.GetComponentsInChildren<Text>().First(x => x.name == "Value");

			UpdateValueText(_effectVolumeValueText, effectVolume);
			_effectVolumeObject.GetComponentInChildren<Slider>().value = effectVolume;

			float musicVolume = AudioManager.GetCategoryVolume(AudioCategory.Music);

			_musicVolumeObject ??= GameObject.Find("MusicVolumeSlider");
			_musicVolumeValueText = _musicVolumeObject.GetComponentsInChildren<Text>().First(x => x.name == "Value");

			UpdateValueText(_musicVolumeValueText, musicVolume);
			_musicVolumeObject.GetComponentInChildren<Slider>().value = musicVolume;
		}

		public override void OnLeave()
		{
			Configuration.Instance.Save();
			base.OnLeave();
		}

		/// <summary>
		/// Sets the effect volume.
		/// Called when the "effect" slider value is changed.
		/// <param name="volume">The new volume sound effects should play at.</param>
		/// </summary>
		public void OnEffectVolumeSliderChanged(float volume)
		{
			Configuration.Instance.EffectVolume = volume;
			UpdateValueText(_effectVolumeValueText, Configuration.Instance.EffectVolume);

			if (!AudioManager.IsPlaying(_scrollAudioId))
				_scrollAudioId = AudioManager.Play(_scrollAudioClip, AudioCategory.Effect);
		}

		/// <summary>
		/// Sets the music volume.
		/// Called when the "music" slider value is changed.
		/// <param name="volume">The new volume sound music should play at.</param>
		/// </summary>
		public void OnMusicVolumeSliderChanged(float volume)
		{
			Configuration.Instance.MusicVolume = volume;
			UpdateValueText(_musicVolumeValueText, Configuration.Instance.MusicVolume);
		}

		/// <summary>
		/// Sets the current menu to the previous menu.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnDoneButtonPressed() => MenuManager.GoBack();

		private void UpdateValueText(Text text, float volume)
		{
			if (text != null)
			{
				volume = (int) Math.Round(volume * 100.0f, 2);
				text.text = $"{volume}%";
			}
		}
	}
}