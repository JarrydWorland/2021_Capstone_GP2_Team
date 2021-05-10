using UnityEngine;
using Player;
using System.Collections;
using System;

//Damage Entitis once, and Decreases Speed.
public class AcidSpill : MonoBehaviour
{
    //Acid does greater Damage on first hit.
    private int _damage = 2;
    private int _meltDamage = 3;
    private float _increaseSpeed = 5.0f;
    private float _decreaseSpeed = 5.0f;

    public void OnTriggerEnter2D(Collider2D hit)
    { 
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                player.GetComponent<Health>().Value -= _damage;
                StartCoroutine(MeltTimer());
                IEnumerator MeltTimer()
                {
                    pScript.Speed -= _decreaseSpeed;
                    for (int i = 0; i < 2; i++)
                    {
                        yield return new WaitForSecondsRealtime(1.0f);
                        player.GetComponent<Health>().Value -= _meltDamage;
                    }
                    yield return new WaitForSecondsRealtime(5.0f);
                    pScript.Speed += _increaseSpeed;
                }
            }
        }
    }
}