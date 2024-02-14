using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTrackPlayer : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _randomWeight = 3f;
    [SerializeField] private bool _y = true;
    private Transform _target;
    private Vector3 _velocity = Vector3.zero;
    private Rigidbody _rigidbody;
    [SerializeField] private float _minBackDistance;
    [SerializeField] private float _faceRestriction;

    [SerializeField] private float _minDistance;
    void Start()
    {
        _target = FindObjectOfType<EthanPlayerMovement>().transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
       if(_target != null)
        {
            float distance = Vector3.Distance(_target.position, transform.position);
            float dot = Vector3.Dot(_target.forward, (transform.position - _target.position).normalized);
            Debug.Log("Dot + " + dot);
            if(dot > _faceRestriction)
            {
                if(distance >= _minDistance)
                {
                    _velocity += (_target.position - transform.position).normalized * _speed;
                    _velocity += Random.insideUnitSphere * _randomWeight;
                    if (!_y)
                        _velocity.y = 0;
                    _velocity = Vector3.ClampMagnitude(_velocity, _speed);
                    transform.rotation = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
                    _rigidbody.velocity = _velocity;

                }
                
            }
            else if(distance <= _minBackDistance)
            {
                if (distance >= _minDistance)
                {
                    _velocity += (_target.position - transform.position).normalized * _speed;
                    _velocity += Random.insideUnitSphere * _randomWeight;
                    if (!_y)
                        _velocity.y = 0;
                    _velocity = Vector3.ClampMagnitude(_velocity, _speed);
                    transform.rotation = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
                    _rigidbody.velocity = _velocity;

                }
            }


        }

        
        
    }
}
