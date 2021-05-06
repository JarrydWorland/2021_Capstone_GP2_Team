using UnityEngine;
using Player;

public class KillPlayerLong : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                StartCoroutine(pScript.CrushTimer());
            }
        }
    }
}
