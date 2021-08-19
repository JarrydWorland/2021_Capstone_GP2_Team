using System;
using UnityEngine;

namespace Scripts.Utilities
{
	public class Lerped<T>
	{
		private Func<T, T, float, T> _easingMethod;
		private float _durationSeconds;
		private float _lastTimeSeconds;
		private bool _unscaledTime;
		private T _current;
		private T _target;

		private float CurrentTime => _unscaledTime ? Time.unscaledTime : Time.time;

		/// <param name="initial">
		/// The value the lerped value should start at.
		/// </param>
		/// <param name="durationSeconds">
		/// How long the lerp should last for in seconds.
		/// </param>
		/// <param name="easingMethod">
		/// The easing method to apply during interpolation, this should be one
		/// of the methods in Scripts.Utilities.Easing
		/// </param>
		/// <param name="unscaledTime">
		/// Whether or not the lerp should ignore time scale.
		/// </param>
		public Lerped(T initial, float durationSeconds, Func<T, T, float, T> easingMethod, bool unscaledTime = false)
		{
			_current = initial;
			_target = initial;
			_durationSeconds = durationSeconds;
			_easingMethod = easingMethod;
			_unscaledTime = unscaledTime;
		}

		/// <summary>
		/// get returns the value lerped between the previous and current
		/// values over _durationSeconds.
		///
		/// set will change the target of the lerp and reset the internal timer
		/// so that the lerp starts again.
		/// </summary>
		public T Value
		{
			get => _easingMethod(_current, _target, Interpolation);
			set
			{
				_current = Value;
				_target = value;
				_lastTimeSeconds = CurrentTime;
			}
		}

		/// <summary>
		/// The current interpolation value used by Value between 0.0f and 1.0f.
		/// </summary>
		public float Interpolation => Mathf.Min((CurrentTime - _lastTimeSeconds) / _durationSeconds, 1.0f);
	}

	public static class Easing
	{
		public static Vector2 Linear(Vector2 start, Vector2 end, float interpolation) => Vector2.LerpUnclamped(start, end, interpolation);
		public static Vector2 EaseInOut(Vector2 start, Vector2 end, float interpolation) => Vector2.LerpUnclamped(start, end, EaseInOut(interpolation));
		public static Vector3 Linear(Vector3 start, Vector3 end, float interpolation) => Vector3.LerpUnclamped(start, end, interpolation);
		public static Vector3 EaseInOut(Vector3 start, Vector3 end, float interpolation) => Vector3.LerpUnclamped(start, end, EaseInOut(interpolation));

		// https://www.desmos.com/calculator/za8sugou91
		private static float EaseInOut(float n) => n <= 0.5
			? +2 * Mathf.Pow(n, 2)
			: -2 * Mathf.Pow(n - 1, 2) + 1;
	}
}