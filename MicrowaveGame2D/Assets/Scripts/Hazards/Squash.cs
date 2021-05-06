using UnityEngine;
using Player;
using System.Collections;

//Damage Entitis once, will eventually drop items.
public class Squash : MonoBehaviour
{
    private float _damage = 0.3f;

    public IEnumerator Damage()
    {
        float Health = GetComponent<Health>().Value;
        yield return new WaitForSecondsRealtime(10.0f);
        Health -= _damage;
    }

    public void OnTriggerEnter2D(Collider2D hit)
    {
        float Health = GetComponent<Health>().Value;
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                StartCoroutine(Damage());
                //Eventully will drop items here.
            }
        }
    }
}