using UnityEngine;
using Player;

//Temporary Code, will ventually kill all entities
public class KillEntities : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            //Will eventually be upgraded to also interact with enemy entities
            if (pScript)
            {
                pScript.Speed = 5.0f;
                player.GetComponent<Health>().Value = 0;
            }
        }
    }
}