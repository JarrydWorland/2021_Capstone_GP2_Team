using UnityEngine;
using Player;
using System.Collections;

//Damage Entitis once, will eventually drop items.
public class Squash : MonoBehaviour
{
    private int _damage = 2;

    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                StartCoroutine(Damage());
                IEnumerator Damage()
                {
                    player.GetComponent<Health>().Value -= _damage;
                    yield return new WaitForSecondsRealtime(0.5f);
                    player.GetComponent<Health>().Value -= _damage;
                    //Eventully will drop items here.
                }
            }
        }
    }
}