using UnityEngine;

// rename to RapidFireWeapon? MachineGun? something?
public class PreAlphaEnemy : MonoBehaviour
{
	[Tooltip("Type of projectile to use")]
	public GameObject projectilePrefab;

	[Tooltip("Fire rate in projectiles per second")]
	public float FireRate = 1f;

	private GameObject _target;
	private Health _health;
	private float _fireRateInverse;
	private float _fireTimer;

	private void Start()
	{
		_target = GameObject.Find("Player");
		_health = GetComponent<Health>();
 		_fireRateInverse = 1.0f / FireRate;
		_fireTimer = 0.0f;
	}

	private void Update()
	{
		UpdateShootLogic();
		UpdateHealthLogic();
	}

	private void UpdateShootLogic()
	{
		_fireTimer += Time.deltaTime;
		if(_fireTimer >= _fireRateInverse)
		{
			Shoot();
			_fireTimer -= _fireRateInverse;
		}
	}

	private void UpdateHealthLogic()
	{
		if (_health.Value == 0)
		{
			Destroy(gameObject);
		}
	}

	private void Shoot()
	{
		GameObject bullet = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
		bullet.GetComponent<PreAlphaProjectile>().Init(gameObject, _target);
	}
}
