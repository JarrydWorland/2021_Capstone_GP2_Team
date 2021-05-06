using UnityEngine;
using Player;

//Damage Entitis once, and Decreases Speed.
public class AcidSpill : MonoBehaviour
{
    //Acid does greater Damage on first hit.
    private float _damage = 0.4f;

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