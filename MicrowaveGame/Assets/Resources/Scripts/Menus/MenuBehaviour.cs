using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts.Menus
{
	/// <summary>
	/// The base menu behaviour to extend off of.
	/// </summary>
	public abstract class MenuBehaviour : MonoBehaviour
	{
		public Selectable CurrentSelectable { get; set; }

		/// <summary>
		/// Called when entering the menu.
		/// </summary>
		public virtual void OnEnter()
		{
			CurrentSelectable = GetComponentsInChildren<Selectable>().First();
			CurrentSelectable.Select();
		}

		/// <summary>
		/// Called when leaving the menu.
		/// </summary>
		public virtual void OnLeave()
		{
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

			if (currentSelectedGameObject != null)
				CurrentSelectable = currentSelectedGameObject.GetComponent<Selectable>();
		}

		/// <summary>
		/// Called when returning to a menu via GoBack().
		/// </summary>
		public virtual void OnReturn()
		{
			CurrentSelectable.Select();
		}
	}
}