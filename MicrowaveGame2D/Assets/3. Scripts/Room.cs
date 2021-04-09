using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
	public IEnumerable<Doorway> Doorways
	{
		get => GetComponentsInChildren<Doorway>(true);
	}

	public HashSet<Direction> Directions
	{
		get => new HashSet<Direction>(Doorways.Select(doorway => doorway.Direction));
	}
	
    // Start is called before the first frame update
    void Start()
    {
		//print(gameObject.GetBounds());
		//print(Doorways.ToList());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
