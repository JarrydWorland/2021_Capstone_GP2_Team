using UnityEngine;

namespace Scripts.Menus
{
	/// <summary>
	/// The base menu behaviour to extend off of.
	/// </summary>
	public abstract class MenuBehaviour : MonoBehaviour
	{
		/// <summary>
		/// Called when entering the menu.
		/// </summary>
		public virtual void OnEnter() { }

		/// <summary>
		/// Called when leaving the menu.
		/// </summary>
		public virtual void OnLeave() { }
	}
}