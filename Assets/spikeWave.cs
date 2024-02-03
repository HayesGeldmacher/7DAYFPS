using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spikeWave : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _damage;


    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.gameObject.tag == "Player")
            {
                
            }
            else
            {
                nearbyObject.gameObject.GetComponent<Health>()?.TakeDamage(_damage);
            }
        }
        GameManager.instance.CallBloomOut();



    }
}
