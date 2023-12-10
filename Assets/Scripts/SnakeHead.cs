using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakeSegment
{
    [Header("Snake Movement")]
    public float DesiredDistance = 10;
    public float Strength = 1;
    public float Squirm = 1;
    [Header("Attack")]
    public float MeanTimeBetweenAttacks = 15f;
    public float AttackTimeVariance = 5f;
    private Transform _target;
    private bool _attacking = false;
    private float _noiseOffset = 0f;
    private float _attackTimer = 0f;

    private void Start()
    {
        InitializePath();
        _target = GameObject.FindObjectOfType<EthanPlayerMovement>().transform;
        _noiseOffset = Random.Range(0f, 100f);
        _attackTimer = MeanTimeBetweenAttacks + Random.Range(-AttackTimeVariance, AttackTimeVariance);
    }

    private void Update()
    {
        UpdatePath();

        _attackTimer -= Time.deltaTime;
        if (_attackTimer < 0 && !_attacking)
            StartCoroutine(Attack());

        if (!_attacking)
        {
            Vector3 toTarget = _target.position - transform.position;
            Vector3 currentVelocity = transform.forward;

            // Desired velocity should be normal to a shpere centered at the target
            Vector3 desiredVelocity = Vector3.ProjectOnPlane(currentVelocity, _target.position - transform.position).normalized;
            // Set velocity maintain desired distance from target
            desiredVelocity += (toTarget.magnitude - DesiredDistance) * toTarget.normalized * Strength;
            // Add some noisy movement perpendicular to the forward direction and towards the target
            desiredVelocity += Vector3.Cross(transform.forward, toTarget).normalized * (Mathf.PerlinNoise(Time.time + _noiseOffset, 0) * 2 - 1) * Squirm / 100;

            // Update position and rotation based on desired velocity
            transform.rotation = Quaternion.LookRotation(desiredVelocity, Vector3.up);
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
    }

    private IEnumerator Attack()
    {
        _attacking = true;
        float prevSpeed = Speed;
        SetSpeed(Speed * 3);
        
        // Move to the target
        Vector3 targetPosition = _target.position;
        Vector3 toTarget = targetPosition - transform.position;
        Vector3 destination = transform.position + toTarget + toTarget.normalized * DesiredDistance;
        transform.rotation = Quaternion.LookRotation(toTarget, Vector3.up);

        while (Vector3.Distance(transform.position, targetPosition) > .1f)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            yield return null;
        }
        while (Vector3.Distance(transform.position, _target.position) < DesiredDistance)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            yield return null;
        }

        _attacking = false;
        _attackTimer = MeanTimeBetweenAttacks + Random.Range(-AttackTimeVariance, AttackTimeVariance);
        
        yield return new WaitForSeconds(1f);
        SetSpeed(prevSpeed);
    }
}
