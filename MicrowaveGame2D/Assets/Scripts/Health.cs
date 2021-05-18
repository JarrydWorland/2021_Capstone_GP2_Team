using System;
using System.Collections.Generic;
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

	private const float _flashSpeed = 10f;
	private float _flashTimer;
	private List<SpriteRenderer> _spriteRenderers
	{
		get
		{
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
			List<SpriteRenderer> childSpriteRenderers = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
			if (spriteRenderer) childSpriteRenderers.Add(spriteRenderer);
			return childSpriteRenderers;
		}
	}

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
			_flashTimer = 0.0f;
			EventManager.Emit(new HealthChangedEventArgs
			{
				GameObject = gameObject,
				OldValue = oldValue,
				NewValue = _value,
			});
		}
	}

	private void Start()
	{
		Value = MaxHealth;
		_flashTimer = 1f;
	}

	private void Update()
	{
		_flashTimer += _flashSpeed * Time.deltaTime;
		float white = 1f - Mathf.Min(_flashTimer, 1f);
		foreach (SpriteRenderer spriteRenderer in _spriteRenderers)
		{
			spriteRenderer.color = new Color(white, white, white);
		}
	}
	
}

public class HealthChangedEventArgs : EventArgs
{
	public GameObject GameObject;
	public int OldValue;
	public int NewValue;
}
