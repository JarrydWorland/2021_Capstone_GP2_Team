using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SpawnFlameRight : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Up());
    }
    public GameObject flame;

    public void InstantiateFlame()
    {
        GameObject _flame = Instantiate(flame) as GameObject;
        _flame.transform.position = new Vector3(transform.position.x +1, transform.position.y);
        Destroy(_flame, 2);
    }

    IEnumerator Up()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(3.0f);
            InstantiateFlame();

        }
    }
}
