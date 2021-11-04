using System;
using Scripts.Rooms;
using Scripts.Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuGameOverBehaviour : MenuBehaviour
	{
		private Text _statisticsText;

		/// <summary>
		/// Sets the current scene to the "Hub" scene.
		/// Called when the "Done" button is pressed.
		/// </summary>
		public void OnDoneButtonPressed() => SceneFaderBehaviour.Instance.FadeInto("Hub");

		private void OnEnable()
		{
			StatisticsTrackerBehaviour statisticsTrackerBehaviour =
				GameObject.Find("StatisticsTracker").GetComponent<StatisticsTrackerBehaviour>();

			_statisticsText = transform.Find("StatisticsText").GetComponent<Text>();

			TimeSpan timeTaken =
				TimeSpan.FromMilliseconds(Time.time * 1000 - statisticsTrackerBehaviour.StartTime * 1000);

			int totalRooms = FindObjectsOfType<RoomConnectionBehaviour>(true).Length;

			float roomsExploredPercentage =
				(float) Math.Round(statisticsTrackerBehaviour.RoomsExplored / (float) totalRooms * 100, 2);

			/*
				Took {0} minutes, {1} seconds
				Defeated {2} enemies
				Dealt {3} damage
				Took {4} damage
				Explored {5} of {6} rooms ({7}%)
			 */

			_statisticsText.text = string.Format(_statisticsText.text, timeTaken.Minutes, timeTaken.Seconds,
				statisticsTrackerBehaviour.EnemiesDefeated, statisticsTrackerBehaviour.DamageDealt,
				statisticsTrackerBehaviour.DamageTaken, totalRooms,
				statisticsTrackerBehaviour.RoomsExplored, roomsExploredPercentage);
		}
	}
}