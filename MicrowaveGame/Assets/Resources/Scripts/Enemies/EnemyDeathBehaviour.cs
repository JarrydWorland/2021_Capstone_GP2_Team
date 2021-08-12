using UnityEngine;

namespace Scripts.Enemies
{
	[RequireComponent(typeof(HealthBehaviour))]
	class EnemyDeathBehaviour : MonoBehaviour
	{
		private HealthBehaviour _healthBehaviour;

		private void Start()
		{
			_healthBehaviour = GetComponent<HealthBehaviour>();
		}



		private void Update()
		{
			if (_healthBehaviour.Value == 0)
			{
				Destroy(gameObject);
			}
		}
	}
}
