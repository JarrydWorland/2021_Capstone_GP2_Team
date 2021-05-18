using UnityEngine;
using Player;
using System.Collections;

public class OilSpill : MonoBehaviour
{
    private float _increaseSpeed = 15.0f;
    private float _decreaseSpeed = 10.0f;
    public void OnTriggerEnter2D(Collider2D hit)
    {
        if (hit.GetComponent<PlayerMovement>() != null)
        {
            GameObject player = hit.gameObject;
            PlayerMovement pScript = player.GetComponent<PlayerMovement>();

            if (pScript)
            {
                StartCoroutine(SpeedTimer());
                IEnumerator SpeedTimer()
                {
                    pScript.Speed = _increaseSpeed;
                    yield return new WaitForSecondsRealtime(5.0f);
                    pScript.Speed = _decreaseSpeed;
                }
            }
        }
    }
}
