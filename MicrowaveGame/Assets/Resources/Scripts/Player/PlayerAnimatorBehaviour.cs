using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Player
{
	[RequireComponent(typeof(PlayerMovementBehaviour), typeof(PlayerShootBehaviour), typeof(Animator))]
	public class PlayerAnimatorBehaviour : MonoBehaviour
	{
		private PlayerMovementBehaviour _playerMovementBehaviour;
		private PlayerShootBehaviour _playerShootBehaviour;
		private Animator _animator;

		private void Start()
		{
			_playerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
			_playerShootBehaviour = GetComponent<PlayerShootBehaviour>();
			_animator = GetComponent<Animator>();
		}

		private void Update()
		{
			UpdateDirection();
			UpdateSpeed();
			UpdateShootingState();
		}

		private void UpdateDirection()
		{
			float speed = _playerMovementBehaviour.Direction.sqrMagnitude;
			if (_playerShootBehaviour.Shooting)
			{
				Vector2 direction = _playerShootBehaviour.RawDirection.ToDirection().ToVector2();
				_animator.SetFloat("DirectionX", direction.x);
				_animator.SetFloat("DirectionY", direction.y);
			}
			else if (speed > 0.0f)
			{
				Vector2 direction = _playerMovementBehaviour.Direction;
				_animator.SetFloat("DirectionX", direction.x);
				_animator.SetFloat("DirectionY", direction.y);
			}
		}

		private void UpdateSpeed()
		{
			// NOTE: speed is normalized between 0.0 and 1.0
			float speed = _playerMovementBehaviour.Direction.sqrMagnitude;
			_animator.SetFloat("Speed", speed);
		}

		private void UpdateShootingState()
		{
			_animator.SetBool("Shooting", _playerShootBehaviour.Shooting);
			_animator.SetFloat("FireRate", _playerShootBehaviour.FireRate);
		}
	}
}
