using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Player
{
	[RequireComponent(typeof(PlayerMovementBehaviour), typeof(PlayerWeaponBehaviour), typeof(Animator))]
	public class PlayerAnimatorBehaviour : MonoBehaviour
	{
		private PlayerMovementBehaviour _playerMovementBehaviour;
		private PlayerWeaponBehaviour _playerWeaponBehaviour;
		private Animator _animator;
		private int _animatorShootingLayer;

		private void Start()
		{
			_playerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
			_playerWeaponBehaviour = GetComponent<PlayerWeaponBehaviour>();
			_animator = GetComponent<Animator>();
			_animatorShootingLayer = _animator.GetLayerIndex("Shooting Layer");
		}

		private void Update()
		{
			UpdateMovementAnimation();
			UpdateShootingAnimation();
		}

		private void UpdateMovementAnimation()
		{
			float speedPercentage = _playerMovementBehaviour.Direction.sqrMagnitude;
			_animator.SetFloat("Speed", speedPercentage);
			if (speedPercentage > 0)
			{
				_animator.SetFloat("MovementDirectionX", _playerMovementBehaviour.Direction.x);
				_animator.SetFloat("MovementDirectionY", _playerMovementBehaviour.Direction.y);
			}
		}

		private void UpdateShootingAnimation()
		{
			Vector2 shootingDirection =  _playerWeaponBehaviour.InputDirection.ToDirection().ToVector2();
			_animator.SetLayerWeight(_animatorShootingLayer, _playerWeaponBehaviour.Shooting ? 1.0f : 0.0f);
			_animator.SetFloat("ShootingDirectionX", shootingDirection.x);
			_animator.SetFloat("ShootingDirectionY", shootingDirection.y);
		}
	}
}
