using UnityEngine;
using Player;
using UnityEngine.InputSystem;
using System.Collections;

public class ConveyorBeltUp : MonoBehaviour
{

    //Move Entities Above
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                //update the position to _target.position
                pScript.transform.position = new Vector3(transform.position.x, transform.position.y + 2);
            }
        }
    }
}