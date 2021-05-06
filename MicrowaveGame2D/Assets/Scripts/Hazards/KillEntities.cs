using UnityEngine;
using Player;

//Temporary Code, will ventually kill all entities
public class KillEntities : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D hit)
    {
        float Health = GetComponent<Health>().Value;
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            //Will eventually be upgraded to also interact with enemy entities
            if (pScript)
            {
                Health -= Health;
            }
        }
    }
}