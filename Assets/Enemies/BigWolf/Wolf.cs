using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wolf : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _cohesionRadius = 5f;
    [SerializeField] private float _separationRadius = 1f;
    [SerializeField] private float _alignmentRadius = 5f;
    [SerializeField] private float _cohesionWeight = 1f;
    [SerializeField] private float _separationWeight = 1f;
    [SerializeField] private float _alignmentWeight = 1f;
    [SerializeField] private float _randomWeight = 3f;
    [SerializeField] private Animator _animator;
    private Transform _target;
    private Vector3 _velocity = Vector3.zero;
    private Rigidbody _rigidbody;
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _target = GameObject.FindObjectOfType<EthanPlayerMovement>().transform;
    }

    void Update()
    {
        // // Get all other wolfs
        // Wolf[] wolfs = FindObjectsOfType<Wolf>();
        
        // // do boids
        // Vector3 cohesion = Vector3.zero;
        // Vector3 separation = Vector3.zero;
        // Vector3 alignment = Vector3.zero;
        // int cohesionCount = 0;
        // int separationCount = 0;
        // int alignmentCount = 0;

        // foreach (Wolf wolf in wolfs)
        // {
        //     if (wolf != this)
        //     {
        //         float distance = Vector3.Distance(transform.position, wolf.transform.position);
        //         if (distance < _cohesionRadius)
        //         {
        //             cohesion += wolf.transform.position;
        //             cohesionCount++;
        //         }
        //         if (distance < _separationRadius)
        //         {
        //             separation += transform.position - wolf.transform.position;
        //             separationCount++;
        //         }
        //         if (distance < _alignmentRadius)
        //         {
        //             alignment += wolf._velocity;
        //             alignmentCount++;
        //         }
        //     }
        // }

        // if (cohesionCount > 0)
        // {
        //     cohesion /= cohesionCount;
        //     cohesion -= transform.position;
        // }

        // if (separationCount > 0)
        // {
        //     separation /= separationCount;
        // }

        // if (alignmentCount > 0)
        // {
        //     alignment /= alignmentCount;
        // }

        // // add target
        // cohesion += _target.position - transform.position;

        // // apply weights
        // cohesion *= _cohesionWeight;
        // separation *= _separationWeight;
        // alignment *= _alignmentWeight;

        // // add random noise
        // cohesion += Random.insideUnitSphere * _randomWeight;
        // separation += Random.insideUnitSphere * _randomWeight;
        // alignment += Random.insideUnitSphere * _randomWeight;

        // apply forces
        // _velocity += cohesion + separation + alignment;
        // transform.position += _velocity * Time.deltaTime;

        _velocity += (_target.position - transform.position).normalized * _speed;
        _velocity += Random.insideUnitSphere * _randomWeight;
        _velocity.y = 0;
        _velocity = Vector3.ClampMagnitude(_velocity, _speed);
        transform.rotation = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);
        _rigidbody.velocity = _velocity;
    }
}