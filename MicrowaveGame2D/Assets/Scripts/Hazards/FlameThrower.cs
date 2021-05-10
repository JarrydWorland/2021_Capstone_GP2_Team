using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class FlameThrower : MonoBehaviour
{
    private int _damage = 2;
    private int _meltDamage = 1;
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
                    pScript.Speed += _increaseSpeed;
                }
            }
        }
    }
}
