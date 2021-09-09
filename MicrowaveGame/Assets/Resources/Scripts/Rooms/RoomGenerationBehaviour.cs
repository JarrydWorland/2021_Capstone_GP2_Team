using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Scripts.Rooms
{
	[RequireComponent(typeof(RoomConnectionBehaviour))]
	public class RoomGenerationBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Whether this room prefab should be used in level generation.
		/// </summary>
		public bool Enabled = true;

		/// <summary>
		/// The spawn probability settings to use for each depthPercentage value.
		///
		/// Ensure these are sorted by DepthPercentage from smallest to largest
		/// in the unity inspector.
		/// </summary>
		public List<SpawnProbability> SpawnProbabilities;

		/// <summary>
		/// Given a depthPercentage, find the SpawnProbability that should be
		/// used for that level generation depth.
		///
		/// A SpawnProbability should be used for a given depth if it is within
		/// the range of the current SpawnProbabilities DepthPercentage and the
		/// next SpawnProbabilities DepthPercentage.
		/// 
		/// For example. If there is one SpawnProbability with a
		/// DepthPercentage of 0.5f, the SpawnProbability settings will only be
		/// applied when the level generation depth is between 0.5f to 1.0f.
		/// </summary>
		/// <param name="depthPercentage">
		/// The current level generation depth as a percentage value
		/// represented between 0.0f and 1.0f
		/// </param>
		/// <returns>
		/// The SpawnProbability containing settings that should be used for
		/// the given depth. Will return null if no SpawnProbability should
		/// apply to the given depth.
		/// </returns>
		public SpawnProbability? GetSpawnProbability(float depthPercentage)
		{
			if (SpawnProbabilities.Count == 0 || SpawnProbabilities[0].DepthPercentage > depthPercentage) return null;

			SpawnProbability currentSpawnProbability = SpawnProbabilities[0];
			foreach (SpawnProbability nextSpawnProbability in SpawnProbabilities.Skip(1))
			{
				if (nextSpawnProbability.DepthPercentage > depthPercentage) break;
				currentSpawnProbability = nextSpawnProbability;
			}

			return currentSpawnProbability;
		}


		[Serializable]
		public struct SpawnProbability
		{
			[SerializeField, Range(0.0f, 1.0f)]
			public float DepthPercentage;
			public float ProbabilityMultiplier;
			public SpawnGuarantee Guarantee;

			public enum SpawnGuarantee
			{
				None,
				SpawnAtLeastOnce,
				SpawnOnlyOnce,
			}
		}
	}
}
