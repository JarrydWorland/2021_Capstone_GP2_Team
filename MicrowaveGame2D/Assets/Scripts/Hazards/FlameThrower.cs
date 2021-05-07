using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class FlameThrower : MonoBehaviour
{
    private float _damage = 0.2f;
    private float _meltDamage = 0.1f;
    private float _increaseSpeed = 5.0f;
    private float _decreaseSpeed = 5.0f;
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();
            float health = player.GetComponent<Health>().Value;

            if (pScript)
            {
                health -= _damage;
                StartCoroutine(MeltTimer());
                IEnumerator MeltTimer()
                {
                    pScript.Speed -= _decreaseSpeed;
                    yield return new WaitForSecondsRealtime(1.0f);
                    health -= _meltDamage;
                    yield return new WaitForSecondsRealtime(1.0f);
                    health -= _meltDamage;
                    yield return new WaitForSecondsRealtime(1.0f);
                    pScript.Speed += _increaseSpeed;
                }
            }
        }
    }
}
