using System;
using UnityEngine;
using System.Linq;

namespace Scripts.Utilities
{
	[Flags]
	public enum LogCategory
	{
		General         = 1 << 0,
		EventManager    = 1 << 1,
		LevelGeneration = 1 << 2,
		AudioManager    = 1 << 3,
		StatusEffect    = 1 << 4
	}

    public static class Log
	{
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
			Action<object> Log = level switch
			{
				LogLevel.Error   => Debug.LogError,
				LogLevel.Warning => Debug.LogWarning,
				_                => Debug.Log,
			};

			string timeString = Grey($"{DateTime.Now:HH:mm:ss} ({Time.unscaledTime * 1000}ms)");
			string categoriesString = Grey($"[{category}]");
			string fileString = Grey(StackTraceFileString(3));

			Log($"{timeString} {categoriesString} {fileString}\n{message}");
		}

		private enum LogLevel
		{
			Info,
			Warning,
			Error,
		}

#if UNITY_EDITOR
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

		public static string StackTraceFileString(int stackFrame)
		{
			char seperator = Environment.OSVersion.Platform == PlatformID.Win32NT ? '\\' : '/';
			var stackTrace = new System.Diagnostics.StackTrace(true).GetFrame(stackFrame);
			string fileString = $"{stackTrace.GetFileName().Split(seperator).Last()}:{stackTrace.GetFileLineNumber()}";
			return fileString;
		}
#else
		public static string Blue(object obj) => obj.ToString();
		public static string Cyan(object obj) => obj.ToString();
		public static string Green(object obj) => obj.ToString();
		public static string Grey(object obj) => obj.ToString();
		public static string Lime(object obj) => obj.ToString();
		public static string Orange(object obj) => obj.ToString();
		public static string Pink(object obj) => obj.ToString();
		public static string Purple(object obj) => obj.ToString();
		public static string Red(object obj) => obj.ToString();
		public static string White(object obj) => obj.ToString();
		public static string Yellow(object obj) => obj.ToString();

		public static string StackTraceFileString(int stackFrame) => "---DEBUG INFORMATION DISABLED---";
#endif
	}
}
