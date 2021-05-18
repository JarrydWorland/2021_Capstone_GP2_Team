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
                if (tag == "Up")
                {
                        //update the position to _target.position
                        pScript.transform.position = new Vector3(pScript.transform.position.x, transform.position.y + 2);
                }
                if(tag == "Down")
                {
                        //update the position to _target.position
                        pScript.transform.position = new Vector3(pScript.transform.position.x, transform.position.y - 2);
                }
                if(tag == "Left")
                {
                    //update the position to _target.position
                    pScript.transform.position = new Vector3(transform.position.x - 2, transform.position.y);
                }
                if(tag == "Right")
                {
                    //update the position to _target.position
                    pScript.transform.position = new Vector3(transform.position.x + 2, transform.position.y);
                }
            }
        }
    }
}