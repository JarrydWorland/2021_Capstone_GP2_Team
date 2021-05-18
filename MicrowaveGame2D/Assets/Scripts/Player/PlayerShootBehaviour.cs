using UnityEngine;
using Weapons;
using UnityEngine.InputSystem;

namespace Player
{
	public class PlayerShootBehaviour : MonoBehaviour
	{
		[Tooltip("The players default weapon")]
		public BaseWeapon DefaultWeapon;

		private BaseWeapon _weapon;
		private Animator[] _animators;

		private void Start()
		{
			_weapon = DefaultWeapon;
			_animators = GetComponentsInChildren<Animator>();
		}

		private void Update()
		{
			Vector2 mousePosition = Mouse.current.position.ReadValue();
			Vector2 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
			Vector2 direction = mousePositionInWorld - (Vector2) transform.position;
			_weapon.Direction = direction;
		}

		// Called by Unitys input system
		public void OnShoot(InputAction.CallbackContext context)
		{
			if (context.performed) 
			{
				_weapon.Shoot();
				foreach(Animator animator in _animators)
				{
					animator.SetTrigger("Aiming");
				}
			}
			else if (context.canceled)
			{
				_weapon.Holster();
				foreach(Animator animator in _animators)
				{
					animator.SetTrigger("Holster");
				}
			}
		}

		public void SetWeapon(BaseWeapon weapon)
		{
			_weapon = weapon;
		}
	}
}
