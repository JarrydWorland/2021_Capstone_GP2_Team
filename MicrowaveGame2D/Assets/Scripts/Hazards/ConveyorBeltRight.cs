using UnityEngine;
using Player;
using UnityEngine.InputSystem;
using System.Collections;

public class ConveyorBeltRight : MonoBehaviour
{
    public Vector2 toRight;

    //Move Entities to the right
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                //update the position
                pScript.transform.position += Vector3.right * pScript.Speed;
            }  
        }
    }
}
