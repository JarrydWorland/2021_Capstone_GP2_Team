using UnityEngine;
using UnityEngine.SceneManagement;
using Scripts.Rooms;
using Scripts.Camera;

namespace Scripts.Levels
{
	public class LevelGenerationBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The room prefab to use as the starting room.
		/// </summary>
		public GameObject StartingRoomPrefab;

		/// <summary>
		/// The depth of the level to be generated. Depth should be a value
		/// larger than zero. A larger depth value will result in a larger
		/// level being generated.
		/// </summary>
		public int Depth;

		/// <summary>
		/// The seed to use when generating the level. Using zero will disable
		/// the seed and use a random value instead.
		/// </summary>
		public int DebugSeed;

		/// <summary>
		/// This option is for debugging only. If true all rooms will always be
		/// enabled in the scene, meaning all the rooms can be seen within the
		/// unity editor while the game is running. However this also means all
		/// rooms will be enabled and loaded even if the player is not in them.
		/// </summary>
		public bool DebugAlwaysShowRooms;
		

		/// <summary>
		/// The starting room instance.
		/// </summary>
		public GameObject StartingRoom { get; private set; }

		private void Start()
		{
			// if no rooms have been manually placed the level will be
			// automatically generated, if rooms have been manually placed the
			// level should put the existing rooms into a valid state.
			
			bool levelGenerationEnabled = FindObjectsOfType<RoomConnectionBehaviour>().Length == 0;
			if (levelGenerationEnabled)
			{
				// no rooms have been manually placed, generate the level.
				int? seed = DebugSeed != 0 ? (int?)DebugSeed : null;
				StartingRoom = LevelGenerator.GenerateLevel(StartingRoomPrefab, transform, Depth, seed, !DebugAlwaysShowRooms);
			}
			else
			{
				// rooms have been manually placed, put them in a valid state.
				StartingRoom = LevelConnector.ConnectLevel(transform);
			}

			// Activate tutorial if it's the player is in the hub scene and
			// they are viewing the narrative menu, meaning it is the first
			// time they are in the hub.
			if (SceneManager.GetActiveScene().name == "Hub" && Persistent.FirstTimeInHub)
			{
				SetupTutorial();
			}

			// set levelTraversalBehaviour.CurrentRoom to StartingRoom
			LevelTraversalBehaviour levelTraversalBehaviour = GetComponent<LevelTraversalBehaviour>();
			if (levelTraversalBehaviour != null)
			{
				levelTraversalBehaviour.CurrentRoom = StartingRoom.GetComponent<RoomConnectionBehaviour>();
			}
		}

		private void SetupTutorial()
		{
			StartingRoom.SetActive(false);
			StartingRoom = transform.Find("TutorialStartN").gameObject;
			StartingRoom.SetActive(true);
			GameObject.Find("Player").transform.position = StartingRoom.transform.position;
			UnityEngine.Camera.main.GetComponent<CameraPanBehaviour>().Position.Value = StartingRoom.transform.position;
		}
	}
}
