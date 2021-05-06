using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class FlameThrower : MonoBehaviour
{
    //private float _damage = 0.2f;
    public void OnTriggerEnter2D(Collider2D hit)
    {
        //float Health = GetComponent<Health>().Value;
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                //Health -= _damage;
                StartCoroutine(pScript.MeltTimer());
            }
        }
    }
}
