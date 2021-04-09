using System;
using System.Linq;
using UnityEngine;
using GameObjectExtensions;


public class Doorway : MonoBehaviour
{
	public Direction Direction
	{
		get
		{
			// GetComponentInParent cant look through inactive objects? >.<
			Room room = GetComponentsInParent<Room>(true).First();
			Bounds doorwayBounds = gameObject.GetBounds();
			Bounds roomBounds = room.gameObject.GetBounds();

			// north (index 0) (distance between north side of doorway and north side of room)
			// east (index 1) (distance between east side of doorway and east side of room)
			// south (index 2) (distance between south side of doorway and south side of room)
			// west (index 3) (distance between west side of doorway and west side of room)
			float[] distances =
			{
				(roomBounds.center.y + roomBounds.extents.y) - (doorwayBounds.center.y + doorwayBounds.extents.y),
				(roomBounds.center.x + roomBounds.extents.x) - (doorwayBounds.center.x + doorwayBounds.extents.x), 
				(doorwayBounds.center.y - doorwayBounds.extents.y) - (roomBounds.center.y - roomBounds.extents.y), 
				(doorwayBounds.center.x - doorwayBounds.extents.x) - (roomBounds.center.x - roomBounds.extents.x), 
			};

			// the doorways direction is the direction with the smallest distance
			// to the "wall", so cast index of minimum distance to its
			// corresponding direction.
			return (Direction)Array.IndexOf(distances, distances.Min());
		}
	}

    // Start is called before the first frame update
    void Start()
    {
		Destroy(GetComponent<SpriteRenderer>());
    }

    // Update is called once per frame
    void Update()
    {

    }

	void OnValidate()
	{

	}
}
