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

		public static void Info(string message, LogCategory categories) => Write(message, LogLevel.Info, categories);

		public static void Warning(string message, LogCategory categories) => Write(message, LogLevel.Warning, categories);

		public static void Error(string message, LogCategory categories) => Write(message, LogLevel.Error, categories);

		private static void Write(string message, LogLevel level, LogCategory categories)
		{
			if (Categories == 0 || (Categories & categories) != Categories) return;

			Action<object> Log = level switch
			{
				LogLevel.Error   => Debug.LogError,
				LogLevel.Warning => Debug.LogWarning,
				_                => Debug.Log,
			};

			Log(message);
		}

		private enum LogLevel
		{
			Info,
			Warning,
			Error,
		}
	}

	[Flags]
	public enum LogCategory
	{
		None            = 0,
		General         = 1 << 0,
		Event           = 1 << 1,
		EventManager    = 1 << 2,
		LevelGeneration = 1 << 3,
	}

	public class EditorLogging : EditorWindow
	{
		private static readonly string[] CategoryNames = Enum.GetNames(typeof(LogCategory));

		private void OnGUI()
		{
			EditorGUILayout.LabelField("Logging Categories");
			LogCategory oldCategories = (LogCategory)EditorPrefs.GetInt("Scripts.Utilities.Log.Categories");
			LogCategory categories = LogCategory.None;
			for (int i=1; i<CategoryNames.Length; i++)
			{
				LogCategory category = (LogCategory)(1 << (i-1));
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
