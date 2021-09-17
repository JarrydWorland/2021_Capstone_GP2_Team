using Scripts.Inventory;
using Scripts.Dialogue;
using Scripts.Menus;
using UnityEngine.SceneManagement;

namespace Scripts.Items
{
	public class ItemKeyCardBehaviour : ItemBehaviour
	{
		private DialogueContentBehaviour _dialogueContentBehaviour;

		public override void Start()
		{
			base.Start();
 			_dialogueContentBehaviour = GetComponent<DialogueContentBehaviour>();
		}

		public override void OnPickupItem(InventorySlotBehaviour inventorySlotBehaviour)
		{
			Persistent.CollectedKeycardCount += 1;
			MenuManager.ShowDialogue(_dialogueContentBehaviour.DialogueContent, () =>
			{
				SceneManager.LoadScene("Hub");
			});
		}

		public override void OnUseItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override void OnUpdateItem(InventorySlotBehaviour inventorySlotBehaviour) { }

		public override bool OnDropItem(InventorySlotBehaviour inventorySlotBehaviour) => true;
	}
}
