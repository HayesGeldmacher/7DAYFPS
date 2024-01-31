using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileProjectile : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _damage;
    [SerializeField] private float _deceleration;
    [SerializeField] private float _maxDistance;
    [SerializeField] private GameObject _bomb;
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
        //Debug.Log("Hit " + other.gameObject.name);
        other.gameObject.GetComponent<Health>()?.TakeDamage(_damage);
        GameObject _explosion = Instantiate(_bomb, other.transform.position, Quaternion.identity);
        DestroyProjectile();
    }
}
