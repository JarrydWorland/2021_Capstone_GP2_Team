using System;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Dialogue;

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
			if (_history.Count == 0) return;

			Current.OnLeave();

			Current.gameObject.SetActive(false);

			Current = _history.Pop();
			Current.gameObject.SetActive(true);

			Current.OnReturn();
		}

		/// <summary>
		/// Given a dialogue object, switch to the dialogue menu and start the dialogue sequence.
		/// </summary>
		/// <param name="dialogue">The dialogue object containing the speaker name and sentences.</param>
		public static void ShowDialogue(DialogueContent dialogue, Action onDialogueComplete = null)
		{
			GoInto("MenuDialogue");

			MenuDialogueBehaviour menuDialogueBehaviour = GameObject.Find("Canvas").transform.Find("MenuDialogue")
				.GetComponent<MenuDialogueBehaviour>();

			menuDialogueBehaviour.StartDialogue(dialogue, onDialogueComplete);
		}
	}
}
