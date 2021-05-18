using UnityEngine;
using Player;
using System.Collections;

public class KillPlayerLong : MonoBehaviour
{
    private bool _in;
    public void OnTriggerEnter2D(Collider2D hit)
    {
        _in = true;
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
                    if(_in)
                    {
                        player.GetComponent<Health>().Value = 0;
                    }
                }
            }
        }
    }
    public void OnTriggerExit2D(Collider2D hit)
    {
        _in = false;
    }
}
