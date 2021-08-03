using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Menus
{
	public static class MenuManager
	{
		private static Stack<MenuBehaviour> _history;

		/// <summary>
		/// The currently active menu.
		/// </summary>
		public static MenuBehaviour Current { get; private set; }

		/// <summary>
		/// Initialises the history stack and sets the current menu to the given menu.
		/// </summary>
		/// <param name="menu">The menu behaviour to be used as the initial menu.</param>
		public static void Init(MenuBehaviour menu)
		{
			_history = new Stack<MenuBehaviour>();
			Current = menu;

			Current.OnEnter();
		}

		/// <summary>
		/// Switches from the current menu to the given menu.
		/// </summary>
		/// <param name="menu">The menu behaviour to switch into.</param>
		public static void GoInto(MenuBehaviour menu)
		{
			Current.OnLeave();

			Current.gameObject.SetActive(false);
			_history.Push(Current);

			Current = menu;
			Current.gameObject.SetActive(true);

			Current.OnEnter();
		}

		/// <summary>
		/// Switches from the current menu to the menu with the given name.
		/// Note that this assumes the menu is inside of a "Canvas" object.
		/// </summary>
		/// <param name="name">The name of the menu to switch into.</param>
		public static void GoInto(string name)
		{
			MenuBehaviour menuBehaviour = GameObject.Find("Canvas").transform.Find(name).GetComponent<MenuBehaviour>();
			GoInto(menuBehaviour);
		}

		/// <summary>
		/// Switches from the current menu to the previous menu.
		/// </summary>
		public static void GoBack()
		{
			Current.OnLeave();

			Current.gameObject.SetActive(false);

			Current = _history.Pop();
			Current.gameObject.SetActive(true);

			Current.OnEnter();
		}
	}
}