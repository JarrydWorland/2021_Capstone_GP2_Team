using UnityEngine;
using Player;
using System.Collections;

//Damage Entitis once, will eventually drop items.
public class Squash : MonoBehaviour
{
    private float _damage = 0.3f;

    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();
            float health = player.GetComponent<Health>().Value;

            if (pScript)
            {
                StartCoroutine(Damage());
                IEnumerator Damage()
                {
                    yield return new WaitForSecondsRealtime(0.5f);
                    health -= _damage;
                }
                //Eventully will drop items here.
            }
        }
    }
}