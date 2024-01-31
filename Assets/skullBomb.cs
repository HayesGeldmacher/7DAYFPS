using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skullBomb : MonoBehaviour
{
    [SerializeField] private float _radius;
    [SerializeField] private float _damage;
    [SerializeField] private float _playerDamage;
    
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);
        foreach (Collider nearbyObject in colliders)
        {
            if(nearbyObject.gameObject.tag == "Player")
            {
                nearbyObject.gameObject.GetComponent<Health>()?.TakeDamage(_playerDamage);
            }
            else
            {
                nearbyObject.gameObject.GetComponent<Health>()?.TakeDamage(_damage);
            }
        }
        GameManager.instance.CallBloomOut();



    }
}
