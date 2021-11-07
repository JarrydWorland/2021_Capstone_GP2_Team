using Scripts.Inventory;
using Scripts.Dialogue;
using Scripts.Menus;
using Scripts.Audio;
using UnityEngine;

namespace Scripts.Items
{
	public class ItemKeyCardBehaviour : ItemBehaviour
	{
		private DialogueContentBehaviour _dialogueContentBehaviour;

		public AudioClip ItemPickup;

		public override void Start()
		{
			base.Start();
			_dialogueContentBehaviour = GetComponent<DialogueContentBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			//Persistent.CollectedKeyCardCount += 1;
			AudioManager.Play(ItemPickup, AudioCategory.Effect, 0.9f);

			MenuManager.ShowDialogue(_dialogueContentBehaviour.DialogueContent,
				() => { MenuManager.GoInto("MenuGameOver"); });
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour) => true;
	}
}