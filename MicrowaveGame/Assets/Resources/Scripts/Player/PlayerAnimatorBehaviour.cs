using UnityEngine;

namespace Scripts.Player
{
	[RequireComponent(typeof(PlayerMovementBehaviour), typeof(Animator))]
	public class PlayerAnimatorBehaviour : MonoBehaviour
	{
		private PlayerMovementBehaviour _playerMovementBehaviour;
		private Animator _animator;

		private void Start()
		{
			_playerMovementBehaviour = GetComponent<PlayerMovementBehaviour>();
			_animator = GetComponent<Animator>();
		}

		private void Update()
		{
			float speedPercentage = _playerMovementBehaviour.Direction.sqrMagnitude;
			_animator.SetFloat("Speed", speedPercentage);
			if (speedPercentage > 0)
			{
				_animator.SetFloat("MovementDirectionX", _playerMovementBehaviour.Direction.x);
				_animator.SetFloat("MovementDirectionY", _playerMovementBehaviour.Direction.y);
			}

			// TODO: this sets the shooting direction towards the origin. This
			// is temporary until weapons are re-implemented.
			Vector2 direction = (Vector3.zero - transform.position).normalized;
			_animator.SetFloat("ShootingDirectionX", direction.x);
			_animator.SetFloat("ShootingDirectionY", direction.y);
		}
	}
}
