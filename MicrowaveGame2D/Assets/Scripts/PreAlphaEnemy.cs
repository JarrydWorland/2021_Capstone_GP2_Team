using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreAlphaEnemy : MonoBehaviour
{
    private Transform target;
    //private Enemy targetEnemy;

    /*[Header("General")]
    public float range = 15f;*/

    [Header("Use Bullets (default)")]
    public GameObject projectilePrefab;
    public float fireRate = 1f;
    private float fireCountdown = 0f;

    /*[Header("Use Laser")]
    public bool useLaser = false;
    public int damageOverTime = 30;
    public float slowAmount = 0.5f;
    public LineRenderer lineRenderer;
    public ParticleSystem impactEffect;
    public Light impactLight;*/

    /*[Header("Unity Setup Fields")]
    //public string enemyTag = "Enemy";
    //public Transform partToRotate;
    //public float turnSpeed = 10f;
    //public Transform firePoint;*/

    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("UpdateTarget", 0f, 0.5f);
        target = GameObject.Find("Player").transform;
    }

    /*void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy != null && shortestDistance <= range)
        {
            target = nearestEnemy.transform;
            //targetEnemy = nearestEnemy.GetComponent<Enemy>();
        }
        else
        {
            target = null;
        }
    }*/

    // Update is called once per frame
    void Update()
    {
        /*if(target == null)
        {
            if(useLaser)
            {
                if(lineRenderer.enabled)
                {
                    lineRenderer.enabled = false;
                    impactEffect.Stop();
                    impactLight.enabled = false;
                }
            }
            return;
        }

        LockOnTarget();

        if(useLaser)
        {
            Laser();
        }
        else
        {
            if(fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f/fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }*/

        if(fireCountdown <= 0f)
        {
            Shoot();
            fireCountdown = 1f/fireRate;
        }

        fireCountdown -= Time.deltaTime;
    }

    /*void LockOnTarget()
    {
        //target lock on
        Vector3 dir = target.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
        partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }*/

    void Shoot()
    {
        //Debug.Log("SHOOT!");
        GameObject bulletGO = (GameObject)Instantiate(projectilePrefab, transform.position, transform.rotation);
        preAlphaProjectile bullet = bulletGO.GetComponent<preAlphaProjectile>();

        if (bullet != null)
        {
            //Debug.Log("seeking target!");
            bullet.Seek(target);
        }
    }

    /*void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }*/
}
