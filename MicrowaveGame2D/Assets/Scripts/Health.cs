using System;
using Events;
using Helpers;
using UnityEngine;

public class Health : MonoBehaviour
{
	/// <summary>
	/// How much health the object has. 1 unit is half a heart, so 6 health is
	/// 3 full hearts.
	/// </summary>
	public int MaxHealth = 6;

	private int _value;

	/// <summary>
	/// The current health of the object.
	/// </summary>
	public int Value
	{
		get => _value;
		set
		{
			int oldValue = _value;
			_value = value.Clamp(0, MaxHealth);
			EventManager.Emit(new HealthChangedEventArgs
			{
				GameObject = gameObject,
				OldValue = oldValue,
				NewValue = _value,
			});
		}
	}

	void Start()
	{
		Value = MaxHealth;
	}
}

public class HealthChangedEventArgs : EventArgs
{
	public GameObject GameObject;
	public int OldValue;
	public int NewValue;
}
