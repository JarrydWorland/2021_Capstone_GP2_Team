using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class preAlphaProjectile : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public int damage = 1;
    //public float explosionRadius = 0f;
    //public GameObject impactEffect;
    private Vector3 dir;
    private Vector3 originPoint;

    public void Seek(Transform _target)
    {
        target = _target;
        dir = target.position - transform.position;
    }

    void start()
    {
        originPoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        float distanceThisFrame = speed * Time.deltaTime;
        /*Vector3 distanceToPlayer = target.position - transform.position;;
        if (distanceToPlayer.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }*/

        if(GetComponent<BoxCollider2D>().IsTouching(target.GetComponent<BoxCollider2D>()))
        {
            HitTarget();
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        //transform.LookAt(target);

        if((transform.position - originPoint).magnitude > 25f)
        {
            //Debug.Log((transform.position - originPoint).magnitude);
            //Debug.Log("cleaned up");
            Destroy(gameObject);
        }
    }

    void HitTarget()
    {
        //Debug.Log("We hit something");
        /*GameObject effectIns = (GameObject)Instantiate(impactEffect, transform.position, transform.rotation);
        Destroy(effectIns, 2f);
        if(explosionRadius > 0f)
        {
            Explode();
        }
        else
        {*/
            Damage(target);
        //}
        
        Destroy(gameObject);
    }
    
    void Damage(Transform enemy)
    {
        Health h = enemy.GetComponent<Health>();
        if(h != null)
        {
            h.Value -= damage;
        }
        //Destroy(enemy.gameObject);
    }

    /*void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach(Collider collider in colliders)
        {
            if(collider.tag == "Enemy")
            {
                Damage(collider.transform);
            }
        }
    }*/

    /*void OnDrawGizmoSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }*/
}
