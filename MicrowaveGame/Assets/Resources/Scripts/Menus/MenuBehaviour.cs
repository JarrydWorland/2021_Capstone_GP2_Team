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
			// TODO: The first button in "MenuPaused" doesn't animate after already animating.
			// If you enter the pause menu, it will animate the first button (in this case the resume button),
			// but if you resume and pause again, it won't animate it.

			CurrentSelectable = GetComponentsInChildren<Selectable>().FirstOrDefault();
			if (CurrentSelectable != null) CurrentSelectable.Select();
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
			if (CurrentSelectable != null) CurrentSelectable.Select();
		}
	}
}