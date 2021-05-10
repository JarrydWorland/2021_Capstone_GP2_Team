using UnityEngine;
using Player;
using System.Collections;

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
                StartCoroutine(CrushTimer());
                IEnumerator CrushTimer()
                {
                    yield return new WaitForSecondsRealtime(7.0f);
                    player.GetComponent<Health>().Value = 0;
                }
            }
        }
    }
}
