using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cicada : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _cohesionRadius = 5f;
    [SerializeField] private float _separationRadius = 1f;
    [SerializeField] private float _alignmentRadius = 5f;
    [SerializeField] private float _cohesionWeight = 1f;
    [SerializeField] private float _separationWeight = 1f;
    [SerializeField] private float _alignmentWeight = 1f;
    [SerializeField] private float _randomWeight = 3f;
    private Transform _target;
    private Vector3 _velocity = Vector3.zero;
    
    void Start()
    {
        _target = GameObject.FindObjectOfType<EthanPlayerMovement>().transform;
    }

    void Update()
    {
        // Get all other cicadas
        Cicada[] cicadas = FindObjectsOfType<Cicada>();
        
        // do boids
        Vector3 cohesion = Vector3.zero;
        Vector3 separation = Vector3.zero;
        Vector3 alignment = Vector3.zero;
        int cohesionCount = 0;
        int separationCount = 0;
        int alignmentCount = 0;

        foreach (Cicada cicada in cicadas)
        {
            if (cicada != this)
            {
                float distance = Vector3.Distance(transform.position, cicada.transform.position);
                if (distance < _cohesionRadius)
                {
                    cohesion += cicada.transform.position;
                    cohesionCount++;
                }
                if (distance < _separationRadius)
                {
                    separation += transform.position - cicada.transform.position;
                    separationCount++;
                }
                if (distance < _alignmentRadius)
                {
                    alignment += cicada._velocity;
                    alignmentCount++;
                }
            }
        }

        if (cohesionCount > 0)
        {
            cohesion /= cohesionCount;
            cohesion -= transform.position;
        }

        if (separationCount > 0)
        {
            separation /= separationCount;
        }

        if (alignmentCount > 0)
        {
            alignment /= alignmentCount;
        }

        // add target
        cohesion += _target.position - transform.position;

        // apply weights
        cohesion *= _cohesionWeight;
        separation *= _separationWeight;
        alignment *= _alignmentWeight;

        // add random noise
        cohesion += Random.insideUnitSphere * _randomWeight;
        separation += Random.insideUnitSphere * _randomWeight;
        alignment += Random.insideUnitSphere * _randomWeight;

        // apply forces
        _velocity += cohesion + separation + alignment;
        _velocity = Vector3.ClampMagnitude(_velocity, _speed);
        transform.position += _velocity * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(_target.position - transform.position, Vector3.up);

        
    }
}
