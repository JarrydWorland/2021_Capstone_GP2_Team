using UnityEngine;

/// <summary>
/// The health of the GameObject between 0.0f and 1.0f (0% and 100%).
/// A GameObjects health can be accessed via GetComponent<Health>().Value
/// </summary>

public class Health : MonoBehaviour
{
	private float _value = 1.0f;
	public float Value
	{
		get => _value;
		set => _value = value.Clamp(0.0f, 1.0f);
	}
}

