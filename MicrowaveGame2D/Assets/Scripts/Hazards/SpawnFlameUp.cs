using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SpawnFlameUp : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Up());
    }
    public GameObject flame;

    public void InstantiateFlame()
    {
        GameObject _flame = Instantiate(flame) as GameObject;
        _flame.transform.position = new Vector3(transform.position.x, transform.position.y+1);
    }

    IEnumerator Up()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(2.0f);
            InstantiateFlame();
        }
    }
}
