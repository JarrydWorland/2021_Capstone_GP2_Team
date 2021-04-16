using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public IEnumerable<Door> Doors => GetComponentsInChildren<Door>(true);
}