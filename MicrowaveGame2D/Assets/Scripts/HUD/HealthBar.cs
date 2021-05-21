using UnityEngine;
using System.Collections.Generic;
using System;
using Events;
using Helpers;

public class HealthBar : MonoBehaviour
{
	public GameObject Target;
	private Health _health;

	private List<GameObject> _hearts = new List<GameObject>();
	private const float _padding = 1.25f;

	private Sprite _fullHeart;
	private Sprite _halfHeart;
	private Sprite _emptyHeart;

    private void Start()
    {
		_fullHeart = Resources.Load<Sprite>("HUD/FullHeart");
		_halfHeart = Resources.Load<Sprite>("HUD/HalfHeart");
		_emptyHeart = Resources.Load<Sprite>("HUD/EmptyHeart");

		_health = Target.GetComponent<Health>();
		uint maxHearts = (uint)Math.Floor((float)_health.MaxHealth / 2);
		for (uint i=0; i<maxHearts; i++)
		{
			_hearts.Add(InstantiateHeart(i));
		}

		EventManager.Register<HealthChangedEventArgs>(OnHealthChangedEvent);
    }

	private void OnHealthChangedEvent(HealthChangedEventArgs eventArgs)
	{
		if (eventArgs.GameObject == Target)
		{
			int difference = eventArgs.NewValue - eventArgs.OldValue;
			if (difference > 0) PlayHealAnimation(eventArgs);
			else if (difference < 0) PlayDamageAnimation(eventArgs);
			else return; // difference == 0, nothing to animate
		}
	}

	private void PlayHealAnimation(HealthChangedEventArgs eventArgs)
	{
		bool useHalfHeart = eventArgs.NewValue % 2 == 1;
		int newValueHeartCount = eventArgs.NewValue / 2;
		for (int i=0; i<_hearts.Count; i++)
		{
			if (i < newValueHeartCount)
			{
				_hearts[i].GetComponent<HeartAnimator>().Animate(_fullHeart, 1.5f, 150.0f, i * 333.0f);
			}
			else if (i == newValueHeartCount && useHalfHeart)
			{
				_hearts[i].GetComponent<HeartAnimator>().Animate(_halfHeart, 1.5f, 150.0f, i * 333.0f);
			}
		}
	}

	private void PlayDamageAnimation(HealthChangedEventArgs eventArgs)
	{
		int oldValueHeart = (int)Math.Ceiling(eventArgs.OldValue / 2.0f);
		int newValueHeart = (int)Math.Ceiling(eventArgs.NewValue / 2.0f);
		bool useHalfHeart = eventArgs.NewValue % 2 == 1;

		for (int i=oldValueHeart; i>=newValueHeart; i--)
		{
			Sprite heart = _emptyHeart;
			if (i == newValueHeart)
			{
				if (!useHalfHeart) break;
				heart = _halfHeart;
			}
			_hearts[i-1].GetComponent<HeartAnimator>().Animate(heart, 0.5f, 150.0f, (oldValueHeart - i) * 333.0f);
		}
	}

	GameObject InstantiateHeart(uint index)
	{
		GameObject heart = new GameObject();

		// sprite
		SpriteRenderer sprite = heart.AddComponent<SpriteRenderer>();
		sprite.sprite = _fullHeart;

		// transform
		heart.transform.SetParent(transform, false);
		heart.transform.localScale = Vector3.one;
		heart.transform.Translate(index * sprite.bounds.size.x * _padding, 0, 0);

		// animator
		heart.AddComponent<HeartAnimator>();

		return heart;
	}

	private class HeartAnimator : MonoBehaviour
	{
		private SpriteRenderer _sprite;

		private Sprite _targetSprite = null;
		private float _animationTime = 0.0f;
		private float _peakScale = 1.0f;
		private float _scaleSpeedMilliseconds = 1000.0f;
		private float _delayMilliseconds = 0.0f;

		void Start()
		{
			_sprite = GetComponent<SpriteRenderer>();
		}

		void Update()
		{
 			// float that counts in 1 unit per second
			_animationTime += Time.deltaTime;

			// scale _animationTime to a value between 0.0f and 1.0f based on _scaleSpeedMilliseconds
			float time = ((_animationTime * 1000) / _scaleSpeedMilliseconds);
	
			// add delay to time
			time -= _delayMilliseconds / 1000.0f;

			// clamp time between 0.0f and 1.0f
			time = time.Clamp(0.0f, 1.0f);

			// wavelength of sin is PI, so scale animation over one wave
			float pointOnWave = time * (float)Math.PI;

			// calculate scale
			float scale = 1.0f + Mathf.Sin(pointOnWave) * (_peakScale - 1.0f);

			// set scale
			transform.localScale = new Vector3(scale, scale, scale);

			// if delay is over, set the sprite to _targetSprite
			if (time > 0.0f && _targetSprite != null) _sprite.sprite = _targetSprite;
		}

		public void Animate(Sprite sprite, float peakScale = 1.0f, float scaleSpeedMilliseconds = 0.0f, float delayMilliseconds = 0.0f)
		{
			_targetSprite = sprite;
			_animationTime = 0.0f;
			_peakScale = peakScale;
			_scaleSpeedMilliseconds = scaleSpeedMilliseconds;
			_delayMilliseconds = delayMilliseconds;
		}
	}
}
