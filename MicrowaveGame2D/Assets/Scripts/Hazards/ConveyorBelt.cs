using UnityEngine;
using Player;
using UnityEngine.InputSystem;
using System.Collections;

public class ConveyorBelt : MonoBehaviour
{
    //Move Entities Below
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                if (pScript.transform.position.z == 180)
                {
                    //update the position to _target.position
                    pScript.transform.position = new Vector3(transform.position.x, transform.position.y - 2);
                }
                if (pScript.transform.position.z == 90)
                {
                    //update the position to _target.position
                    pScript.transform.position = new Vector3(transform.position.x, transform.position.y + 2);
                }
                if (pScript.transform.position.z == 270)
                {
                    //update the position to _target.position
                    pScript.transform.position = new Vector3(transform.position.x - 2, transform.position.y);
                }
                if (pScript.transform.position.z == 0)
                {
                    //update the position to _target.position
                    pScript.transform.position = new Vector3(transform.position.x + 2, transform.position.y);
                }
            }
        }
    }
}