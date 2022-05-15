using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI.MonsterBehavior;

public class ProjectileMovement : MonoBehaviour
{
    public float projectileSpeed;
    public float minprojectileDamage;
    public float maxprojectileDamage;

    private bool stopCachingTheTarget;

    private GameObject generalTarget;
    private ParticleSystem particle;
    private Vector3 distance;

    private void Start()
    {
        particle = transform.GetChild(0).gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (stopCachingTheTarget)
        {
            transform.Translate(Time.deltaTime * projectileSpeed * distance.normalized, Space.World);
            //particle.startRotation = -transform.rotation.eulerAngles.z / (180.0f / Mathf.PI);
        }
    }

    public void ThrowProjectile(GameObject target)
    {
        if (!stopCachingTheTarget)
        {
            generalTarget = target;
            distance = target.transform.position - transform.position;
            transform.LookAt(target.transform.position);
            stopCachingTheTarget = true;
            Destroy(gameObject, 5);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == generalTarget)
        {
            if (other.gameObject.GetComponent<EnemyBehaviours>() != null)
            {
                other.gameObject.GetComponent<EnemyBehaviours>().GetHittedFromNPC(Random.Range(minprojectileDamage, maxprojectileDamage));
            }
            else if (other.gameObject.GetComponent<PlayerHealth>() != null)
            {
                PlayerHealth.health -= Random.Range(minprojectileDamage, maxprojectileDamage);
            }
        }
    }
}
