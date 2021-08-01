using System.Collections.Generic;

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
		}

		/// <summary>
		/// Switches from the current menu to the given menu.
		/// </summary>
		/// <param name="menu">The menu behaviour to switch into.</param>
		public static void GoInto(MenuBehaviour menu)
		{
			Current.gameObject.SetActive(false);
			_history.Push(Current);

			Current = menu;
			Current.gameObject.SetActive(true);
		}

		/// <summary>
		/// Switches from the current menu to the previous menu.
		/// </summary>
		public static void GoBack()
		{
			Current.gameObject.SetActive(false);

			Current = _history.Pop();
			Current.gameObject.SetActive(true);
		}
	}
}