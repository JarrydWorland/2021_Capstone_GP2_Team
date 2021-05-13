using UnityEngine;
using Player;
using UnityEngine.InputSystem;
using System.Collections;

public class ConveyorBeltLeft : MonoBehaviour
{

    //Move Entities Left
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                //update the position to _target.position
                pScript.transform.position = new Vector3(transform.position.x - 2, transform.position.y);
            }
        }
    }
}