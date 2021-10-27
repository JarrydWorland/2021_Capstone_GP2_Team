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

		private Color setColor = new Color(61, 255, 98, 255);
		private Color makeColor; 

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
			const float margin = 0.9f;
			float cellHeight = CellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
			CellPrefab.GetComponent<SpriteRenderer>().color = setColor;
			for (int i=0; i<_targetHealthBehaviour.MaxHealth; i++)
			{
				GameObject obj = Instantiate(CellPrefab, transform);
				obj.transform.position += new Vector3((cellHeight * margin * i), 0, 0);
				makeColor = obj.GetComponent<SpriteRenderer>().color;
				obj.GetComponent<SpriteRenderer>().color = setColor;
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

		public void AddCells(int IncreaseValue, int newMaxHealth)
        {
			const float margin = 0.9f;
			float cellHeight = CellPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
			CellPrefab.GetComponent<SpriteRenderer>().color = setColor;

			//if (_targetHealthBehaviour.MaxHealth != newMaxHealth)
			//	_targetHealthBehaviour.MaxHealth = newMaxHealth;

			for (int i = _targetHealthBehaviour.MaxHealth; i < (_targetHealthBehaviour.MaxHealth + IncreaseValue); i++)
			{
				GameObject obj = Instantiate(CellPrefab, transform);
				obj.transform.position += new Vector3((cellHeight * margin * i), 0, 0);
				makeColor = obj.GetComponent<SpriteRenderer>().color;
				obj.GetComponent<SpriteRenderer>().color = setColor;
				_cellAnimators.Add(obj.GetComponent<Animator>());
				_cellAnimators[i].SetBool("Full", false);
			}
		}
	}
}
