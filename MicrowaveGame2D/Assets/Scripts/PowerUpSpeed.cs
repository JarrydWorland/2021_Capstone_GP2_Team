using UnityEngine;
using Player;

public class PowerUpSpeed : MonoBehaviour
{
    public float inc = 20.0f;

    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                pScript.Speed += inc;
                Destroy(gameObject);
            }
        }
    }
}
