using System;
using System.IO;
using System.Text;
using Scripts.Audio;
using Scripts.Utilities;
using UnityEngine;

namespace Scripts.Config
{
	public class Configuration
	{
		/// <summary>
		/// The configuration instance.
		/// </summary>
		public static Configuration Instance;

		/// <summary>
		/// The effect volume.
		/// </summary>
		public float EffectVolume
		{
			get => AudioManager.GetCategoryVolume(AudioCategory.Effect);
			set
			{
				value = (float) Math.Round(value, 2);
				AudioManager.SetCategoryVolume(AudioCategory.Effect, value);
			}
		}

		/// <summary>
		/// The music volume.
		/// </summary>
		public float MusicVolume
		{
			get => AudioManager.GetCategoryVolume(AudioCategory.Music);
			set
			{
				value = (float) Math.Round(value, 2);
				AudioManager.SetCategoryVolume(AudioCategory.Music, value);
			}
		}

		private string _path;

		/// <summary>
		/// Writes the current configuration settings to a file.
		/// </summary>
		public void Save()
		{
			using FileStream stream = File.OpenWrite(_path);
			using StreamWriter writer = new StreamWriter(stream);

			SaveSounds(writer);
			writer.Flush();

			Log.Info($"Saved {Log.Blue(_path)} configuration file.");
		}

		private void SaveSounds(StreamWriter writer)
		{
			writer.WriteLine("[Sounds]");
			writer.WriteLine($"EffectVolume={EffectVolume:N2}");
			writer.WriteLine($"MusicVolume={MusicVolume:N2}");
		}

		/// <summary>
		/// Loads a given configuration file and returns its contents in a <see cref="Configuration"/> object.
		/// </summary>
		/// <param name="path">The configuration file to load.</param>
		/// <returns>A <see cref="Configuration"/> object containing the given configuration file's content.</returns>
		public static Configuration Load(string path = null)
		{
			path ??= Application.persistentDataPath + "/config.ini";
			
			Configuration configuration = new Configuration
			{
				_path = path
			};

			if (!File.Exists(path)) return configuration;

			using FileStream stream = File.OpenRead(path);
			using StreamReader reader = new StreamReader(stream);

			string section = string.Empty;

			while (!reader.EndOfStream)
			{
				string line = reader.ReadLine()?.Trim();
				if (string.IsNullOrWhiteSpace(line) || line.StartsWith(";")) continue;

				if (line.StartsWith("[") && line.EndsWith("]"))
				{
					section = line.Substring(1, line.Length - 2);
					continue;
				}

				string[] tokens = line.Split('=');
				StringBuilder valueBuilder = new StringBuilder(tokens[1]);

				for (int i = 2; i < tokens.Length; i++)
				{
					valueBuilder.Append('=');
					valueBuilder.Append(tokens[i]);
				}

				string name = tokens[0], value = valueBuilder.ToString();

				switch (section)
				{
					case "Sounds":
						LoadSounds(configuration, name, value);
						break;
				}
			}

			Log.Info($"Loaded {Log.Blue(path)} configuration file.");
			return configuration;
		}

		private static void LoadSounds(Configuration configuration, string name, string value)
		{
			switch (name)
			{
				case "EffectVolume" when float.TryParse(value, out float effectVolume):
					configuration.EffectVolume = Mathf.Clamp((float) Math.Round(effectVolume, 2), 0.0f, 1.0f);
					break;
				case "MusicVolume" when float.TryParse(value, out float musicVolume):
					configuration.MusicVolume = Mathf.Clamp((float) Math.Round(musicVolume, 2), 0.0f, 1.0f);
					break;
			}
		}
	}
}