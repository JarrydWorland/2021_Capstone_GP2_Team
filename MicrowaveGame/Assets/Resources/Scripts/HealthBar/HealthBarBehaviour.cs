using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using Scripts.Events;

namespace Scripts.HealthBar
{
	public class HealthBarBehaviour : MonoBehaviour
	{
		/// <summary>
		/// How cell prefab to use in the health bar.
		/// </summary>
		public GameObject CellPrefab;

		/// <summary>
		/// The target to watch for health changed events.
		/// </summary>
		public GameObject Target;

		private HealthBehaviour _targetHealthBehaviour;
		private EventId<HealthChangedEventArgs> _healthChangedEventId;
		List<Animator> _cellAnimators = new List<Animator>();

		private void Start()
		{
			_targetHealthBehaviour = Target.GetComponent<HealthBehaviour>();
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
			CreateCells();
		}

		private void OnDestroy() => EventManager.Unregister(_healthChangedEventId);

		private void CreateCells()
		{
			const float margin = 0.75f;
			float cellHeight = CellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
			for (int i=0; i<_targetHealthBehaviour.MaxHealth; i++)
			{
				GameObject obj = Instantiate(CellPrefab, transform);
				obj.transform.position += new Vector3(0, (cellHeight * margin * i), 0);
				_cellAnimators.Add(obj.GetComponent<Animator>());
			}
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject.name != "Player") return;

			for (int i=0; i<_targetHealthBehaviour.MaxHealth; i++)
			{
				if (i < eventArgs.NewValue) _cellAnimators[i].SetBool("Full", true);
				else _cellAnimators[i].SetBool("Full", false);
			}
		}
	}
}
