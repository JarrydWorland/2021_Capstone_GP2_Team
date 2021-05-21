using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class SpawnFlame : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Up());
        StartCoroutine(Down());
        StartCoroutine(Left());
        StartCoroutine(Right());
    }
    public GameObject flame;

    public void InstantiateFlameUp()
    {
        GameObject _flame = Instantiate(flame) as GameObject;
        _flame.transform.position = new Vector3(transform.position.x, transform.position.y + 1);
        Destroy(_flame, 2);
    }

    public void InstantiateFlameDown()
    {
        GameObject _flame = Instantiate(flame) as GameObject;
        _flame.transform.position = new Vector3(transform.position.x, transform.position.y - 1);
        Destroy(_flame, 2);
    }

    public void InstantiateFlameLeft()
    {
        GameObject _flame = Instantiate(flame) as GameObject;
        _flame.transform.position = new Vector3(transform.position.x - 1, transform.position.y);
        Destroy(_flame, 2);
    }

    public void InstantiateFlameRight()
    {
        GameObject _flame = Instantiate(flame) as GameObject;
        _flame.transform.position = new Vector3(transform.position.x + 1, transform.position.y);
        Destroy(_flame, 2);
    }

    IEnumerator Up()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(3.0f);
            InstantiateFlameUp();
        }
    }

    IEnumerator Down()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(3.0f);
            InstantiateFlameDown();
        }
    }

    IEnumerator Left()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(3.0f);
            InstantiateFlameLeft();
        }
    }

    IEnumerator Right()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(3.0f);
            InstantiateFlameRight();
        }
    }
}
