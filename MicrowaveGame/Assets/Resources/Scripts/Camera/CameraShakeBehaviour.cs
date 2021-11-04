using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Scripts.Camera
{
	public class CameraShakeBehaviour : MonoBehaviour
	{
		private const float CameraShakeDamping = 50.0f;

		private List<ShakeCommand> _shakeCommands = new List<ShakeCommand>();
		private Vector3 _cameraShakeOffset;

		/// <summary>
		/// Shake the camera at a given strength for a given amount of time.
		/// Multiple camera shakes from different sources will overlap
		/// correctly.
		/// </summary>
		/// <param name="strength">How strongly the camera should shake.</param>
		/// <param name="durationSeconds">How long the camera should shake for in seconds.</param>
		public void Shake(float strength, float durationSeconds)
		{
			ShakeCommand shakeCommand = _shakeCommands.Find(command => command.Strength == strength);
			if (shakeCommand == null)
			{
				_shakeCommands.Add(new ShakeCommand
				{
					Strength = strength,
					ExpireTime = 0,
				});
				shakeCommand = _shakeCommands[_shakeCommands.Count - 1];
			}
			shakeCommand.ExpireTime = Mathf.Max(Time.time + durationSeconds, shakeCommand.ExpireTime);
		}

		private void Update()
		{
			if (_shakeCommands.Count > 0)
			{
				ShakeCamera();
			}
			CullExpiredShakeCommands();
		}

		private float GetShakeStrength()
		{
			return _shakeCommands.Max(command => command.Strength);
		}

		private void CullExpiredShakeCommands()
		{
			int removedCount = _shakeCommands.RemoveAll(command => command.ExpireTime <= Time.time);
			if (_shakeCommands.Count == 0 && removedCount > 0)
			{
				ResetCamera();
			}
		}

		private void ShakeCamera()
		{
			// rotation
			float strength = GetShakeStrength();
			float zRotation = UnityEngine.Random.Range(0.0f, strength) - (strength/2.0f);
			transform.rotation = Quaternion.Lerp(
				UnityEngine.Camera.main.transform.rotation,
				Quaternion.Euler(0, 0, zRotation),
				CameraShakeDamping * Time.deltaTime
			);

			// position
			strength /= CameraShakeDamping / 2.0f;
			transform.position -= _cameraShakeOffset;
			_cameraShakeOffset = new Vector3
			{
				x = UnityEngine.Random.Range(0.0f, strength) - (strength/2.0f),
				y = UnityEngine.Random.Range(0.0f, strength) - (strength/2.0f),
				z = 0.0f,
			};
			transform.position += _cameraShakeOffset;
		}

		private void ResetCamera()
		{
			transform.rotation = Quaternion.identity;
			transform.position -= _cameraShakeOffset;
			_cameraShakeOffset = Vector3.zero;
		}

		private class ShakeCommand
		{
			public float Strength;
			public float ExpireTime;
		}
	}
}
