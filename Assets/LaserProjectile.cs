using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _maxDistance;
    private Vector3 _startPosition;

    void Start()
    {
        _startPosition = transform.position;
    }

    void Update()
    {
        transform.position += transform.forward * _speed * Time.deltaTime;

        if (Vector3.Distance(_startPosition, transform.position) > _maxDistance)
        {
            DestroyProjectile();
        }

    }

    void DestroyProjectile()
    {
        //get some sort of fading aimation effect instead of just disappearing!
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered!");
        Debug.Log(other.transform);

        if (other.transform.tag == "Player")
        {
           
            other.transform.GetComponent<Health>()?.TakeDamage(_damage);
            DestroyProjectile();
        }
    }


}
