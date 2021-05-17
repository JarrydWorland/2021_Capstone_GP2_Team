using System;
using UnityEngine;

public class PreAlphaProjectile : MonoBehaviour
{
	[Tooltip("Speed the projectile will travel at in units per second")]
	public float Speed = 10f;

	[Tooltip("Half-hearts of damage dealt to the target on impact")]
	public int Damage = 1;

	private GameObject _parent;
	private Vector3 _origin;
	private GameObject _target;
	private Rigidbody2D _rigidbody;
	private Vector2 _direction;

	public PreAlphaProjectile Init(GameObject parent, GameObject target)
	{
		_parent = parent;
		_origin = parent.transform.position;
		_target = target;
		_direction = target.transform.position - transform.position;
		_direction.Normalize();
		return this;
	}

	private void Start()
    {
		_rigidbody = GetComponent<Rigidbody2D>();
    }

	private void FixedUpdate()
	{
		Debug.Assert(_origin != null, "PreAlphaProjectile was not initialized");
		FixedUpdateMoveLogic();
		FixedUpdateDespawnLogic();
	}

	private void FixedUpdateMoveLogic()
	{
		Vector2 position = _rigidbody.position + _direction * (Speed * Time.fixedDeltaTime);
		_rigidbody.MovePosition(position);
	}

	private void FixedUpdateDespawnLogic()
	{
		float distanceFromOrigin = (_origin - transform.position).sqrMagnitude;
		if (distanceFromOrigin > 1000f)
		{
			Destroy(gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject == _parent) return;
		if (other.gameObject == _target)
		{
			other.GetComponent<Health>().Value -= Damage;
		}
		GameObject[] collidables = GameObject.FindGameObjectsWithTag("Collidable");

		foreach (GameObject collidable in collidables)
		{
			if (other.gameObject == collidable) { Destroy(gameObject); }
		}
		Destroy(gameObject);
	}
}
