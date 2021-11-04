using System;
using Scripts.Rooms;
using Scripts.Scenes;
using Scripts.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.Menus
{
	public class MenuGameOverBehaviour : MenuBehaviour
	{
		private Text _statisticsText;

		private const float StatisticsLerpTimeSeconds = 4.0f;
		private Lerped<string> _statisticsTextContent = new Lerped<string>(string.Empty, StatisticsLerpTimeSeconds, Interpolate, true);

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

			_statisticsTextContent.Value = string.Format(_statisticsText.text, timeTaken.Minutes, timeTaken.Seconds,
				statisticsTrackerBehaviour.EnemiesDefeated, statisticsTrackerBehaviour.DamageDealt,
				statisticsTrackerBehaviour.DamageTaken, totalRooms,
				statisticsTrackerBehaviour.RoomsExplored, roomsExploredPercentage);

			_statisticsText.text = string.Empty;
		}

		private void Update()
		{
			_statisticsText.text = _statisticsTextContent.Value;
		}

		private static string Interpolate(string _, string line, float interpolation)
		{
			if (string.IsNullOrWhiteSpace(line)) return string.Empty;
			return line.Substring(0, (int) (line.Length * interpolation));
		}
	}
}
