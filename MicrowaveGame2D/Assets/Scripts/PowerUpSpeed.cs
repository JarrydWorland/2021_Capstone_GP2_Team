using UnityEngine;
using Player;

public class PowerUpSpeed : MonoBehaviour
{
    public float inc = 90.0f;

    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.tag == "Player")
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                pScript.Speed += inc;
            }
        }
    }
}
