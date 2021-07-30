﻿using UnityEngine;

namespace Scripts.Menus
{
	public class MenuCreditsBehaviour : MonoBehaviour
	{
		public void OnDoneButtonPressed()
		{
			gameObject.SetActive(false);
			transform.parent.Find("Main").gameObject.SetActive(true);
		}
	}
}