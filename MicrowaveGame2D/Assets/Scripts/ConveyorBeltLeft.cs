using UnityEngine;
using Player;
using UnityEngine.InputSystem;
using System.Collections;

public class ConveyorBeltLeft : MonoBehaviour
{
    public class ConveyorBelt_Right : MonoBehaviour
    {
        public Vector2 toLeft;

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
                    pScript.Velocity = -transform.right * pScript.Speed;
                }
            }
        }
    }
}
