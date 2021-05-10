using UnityEngine;
using Player;
using System.Collections;

//Damage Entitis once, and Decreases Speed.
public class Nails : MonoBehaviour
{
    private int _damage = 1;
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
                StartCoroutine(SlowTimer());
                IEnumerator SlowTimer()
                {
                    pScript.Speed -= _decreaseSpeed;
                    yield return new WaitForSecondsRealtime(5.0f);
                    pScript.Speed += _increaseSpeed;
                }
            }
        }
    }
}