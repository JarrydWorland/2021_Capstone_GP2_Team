using UnityEngine;
using Scripts.Utilities;

namespace Scripts.Doors
{
	public class DoorConnectionBehaviour : MonoBehaviour
	{
		/// <summary>
		/// The direction the door leads towards relative to the room its in.
		/// For example, a door at the top of the room would be a North door.
		/// </summary>
		public Direction Direction;

		/// <summary>
		/// The sound to play when the door is entered by the
		/// player.
		/// </summary>
		public AudioClip EnterAudioClip;

		/// <summary>
		/// The door sprite to be used when the door is open.
		/// </summary>
		public Sprite OpenDoorSprite;

		/// <summary>
		/// The door sprite to be used when the door is closed.
		/// </summary>
		public Sprite ClosedDoorSprite;

		/// <summary>
		/// The door that this door leads into, usually located in another room.
		/// </summary>
		public DoorConnectionBehaviour ConnectingDoor { get; private set; }

		/// <summary>
		/// Returns true if the door is open.
		/// </summary>
		public bool IsOpen { get; private set; } = true;

		/// <summary>
		/// Opens / unlocks the door.
		/// </summary>
		public void Open()
		{
			IsOpen = true;

			GetComponent<SpriteRenderer>().sprite = OpenDoorSprite;
			GetComponent<Animator>().Play("DoorPulsateOut");
		}

		/// <summary>
		/// Closes / locks the door.
		/// </summary>
		public void Close()
		{
			GetComponent<SpriteRenderer>().sprite = ClosedDoorSprite;
			GetComponent<Animator>().Play("DoorPulsateIn");

			IsOpen = false;
		}

		/// <summary>
		/// Connects two doors together so that they lead into each other. Used to create the level graph.
		/// </summary>
		/// <param name="left">The first door that will connect to the second door.</param>
		/// <param name="right">The second door that will connect to the first door.</param>
		public static void Connect(DoorConnectionBehaviour left, DoorConnectionBehaviour right)
		{
			left.ConnectingDoor = right;
			right.ConnectingDoor = left;
		}
	}
}