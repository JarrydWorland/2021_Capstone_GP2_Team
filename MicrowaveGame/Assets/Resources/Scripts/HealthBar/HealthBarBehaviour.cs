using UnityEngine;
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
		private EventId<HealthMaxChangedEventArgs> _healthMaxChangedEventId;
		List<Animator> _cellAnimators = new List<Animator>();

		private void Start()
		{
			_targetHealthBehaviour = Target.GetComponent<HealthBehaviour>();
			_healthChangedEventId = EventManager.Register<HealthChangedEventArgs>(OnHealthChanged);
			_healthMaxChangedEventId = EventManager.Register<HealthMaxChangedEventArgs>(OnHealthMaxChanged);
			UpdateCells();
		}

        private void OnDestroy()
		{
			EventManager.Unregister(_healthChangedEventId);
			EventManager.Unregister(_healthMaxChangedEventId);
		}

		private void UpdateCells()
		{
			// destroy any existing cells
			_cellAnimators.Clear();
			foreach (Transform child in transform) Destroy(child.gameObject);

			// create cells
			const float margin = 0.9f;
			float cellHeight = CellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
			for (int i=0; i<_targetHealthBehaviour.MaxHealth; i++)
			{
				// instantiate cell
				GameObject obj = Instantiate(CellPrefab, transform);
				obj.transform.position += new Vector3((cellHeight * margin * i), 0, 0);

				// set cell to correct animation state based on current health value
				Animator cellAnimator = obj.GetComponent<Animator>();
				cellAnimator.Play(i < _targetHealthBehaviour.Value ? "HealthBarCellShow" : "HealthBarCellHide");
				cellAnimator.SetBool("Full", i < _targetHealthBehaviour.Value);

				// add animator to animators list
				_cellAnimators.Add(cellAnimator);
			}
		}

		private void OnHealthChanged(HealthChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject != Target) return;

			for (int i=0; i<_targetHealthBehaviour.MaxHealth; i++)
			{
				if (i < eventArgs.NewValue) _cellAnimators[i].SetBool("Full", true);
				else _cellAnimators[i].SetBool("Full", false);
			}
		}

		private void OnHealthMaxChanged(HealthMaxChangedEventArgs eventArgs)
		{
			if (eventArgs.GameObject != Target) return;
			UpdateCells();
		}
	}
}
