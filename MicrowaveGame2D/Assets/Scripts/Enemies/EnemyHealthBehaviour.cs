using UnityEngine;

namespace Enemy
{
	[RequireComponent(typeof(Health))]
	public class EnemyHealthBehaviour : MonoBehaviour
	{
		private Health _health;

		private void Start()
		{
			_health = GetComponent<Health>();
		}

		private void Update()
		{
			if (_health.Value == 0)
			{
				Destroy(gameObject);
			}
		}
	}
}
