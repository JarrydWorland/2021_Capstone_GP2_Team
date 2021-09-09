using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Scripts.Utilities
{
    public static class Log
	{

#if UNITY_EDITOR
		private static readonly LogCategory Categories = (LogCategory)EditorPrefs.GetInt("Scripts.Utilities.Log.Categories");
#else
		public static readonly LogCategory Categories = LogCategory.None;
#endif

		/// <summary>Log something as information.</summary>
		/// <param name="message">The message to log.</param>
		/// <param name="category">
		/// A bitflag representing the categories this log is a part of.
		/// Defaults to only LogCategory.General.
		/// </param>
		public static void Info(string message, LogCategory category = LogCategory.General) => Write(message, LogLevel.Info, category);

		/// <summary>Log something as a warning.</summary>
		/// <param name="message">The message to log.</param>
		/// <param name="category">
		/// A bitflag representing the categories this log is a part of.
		/// Defaults to only LogCategory.General.
		/// </param>
		public static void Warning(string message, LogCategory category = LogCategory.General) => Write(message, LogLevel.Warning, category);

		/// <summary>Log something as an error.</summary>
		/// <param name="message">The message to log.</param>
		/// <param name="category">
		/// A bitflag representing the categories this log is a part of.
		/// Defaults to only LogCategory.General.
		/// </param>
		public static void Error(string message, LogCategory category = LogCategory.General) => Write(message, LogLevel.Error, category);

		private static void Write(string message, LogLevel level, LogCategory category)
		{
			if ((Categories & category) == 0) return; // filter for any category
			// if (Categories == 0 || (Categories & category) != Categories) return; // filter for only category

			Action<object> Log = level switch
			{
				LogLevel.Error   => Debug.LogError,
				LogLevel.Warning => Debug.LogWarning,
				_                => Debug.Log,
			};

			var stackTrace = new System.Diagnostics.StackTrace(true).GetFrame(2);
			string timeString = Grey($"{DateTime.Now:HH:mm:ss} ({Time.unscaledTime * 1000}ms)");
			string categoriesString = Grey($"[{category}]");
			string fileString = Grey($"({stackTrace.GetFileName().Split('\\').Last()}:{stackTrace.GetFileLineNumber()})");
			Log($"{timeString} {categoriesString} {fileString}\n{message}");
		}

		private enum LogLevel
		{
			Info,
			Warning,
			Error,
		}

		public static string Blue(object obj) => $"<color=#2196F3>{obj}</color>";
		public static string Cyan(object obj) => $"<color=#00BCD4>{obj}</color>";
		public static string Green(object obj) => $"<color=#4CAF50>{obj}</color>";
		public static string Grey(object obj) => $"<color=#666666>{obj}</color>";
		public static string Lime(object obj) => $"<color=#8BC34A>{obj}</color>";
		public static string Orange(object obj) => $"<color=#FF9800>{obj}</color>";
		public static string Pink(object obj) => $"<color=#E91E63>{obj}</color>";
		public static string Purple(object obj) => $"<color=#9C27B0>{obj}</color>";
		public static string Red(object obj) => $"<color=#F44336>{obj}</color>";
		public static string White(object obj) => $"<color=#FFFFFF>{obj}</color>";
		public static string Yellow(object obj) => $"<color=#FFEB3B>{obj}</color>";
	}

	[Flags]
	public enum LogCategory
	{
		General         = 1 << 0,
		EventManager    = 1 << 1,
		LevelGeneration = 1 << 2,
		AudioManager    = 1 << 3,
	}

	public class EditorLogging : EditorWindow
	{
		private static readonly string[] CategoryNames = Enum.GetNames(typeof(LogCategory));

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Logging Categories");
			LogCategory oldCategories = (LogCategory)EditorPrefs.GetInt("Scripts.Utilities.Log.Categories");
			LogCategory categories = 0;
			for (int i=0; i<CategoryNames.Length; i++)
			{
				LogCategory category = (LogCategory)(1 << i);
				bool val = EditorGUILayout.Toggle(CategoryNames[i], oldCategories.HasFlag(category));
				if (val) categories |= category;
			}

			EditorPrefs.SetInt("Scripts.Utilities.Log.Categories", (int)categories);
    	}

		[MenuItem("Utilities/Logging")]
		private static void MenuItem()
		{
			EditorLogging window = GetWindow<EditorLogging>(false, "Logging");
			window.Show();
		}
	}
}
