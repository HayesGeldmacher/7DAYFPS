using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeHead : SnakeSegment
{
    [Header("Snake Movement")]
    public float DesiredDistance = 10;
    public float DesiredHeight = 5;
    public float CorrectionStrength = .2f;
    public float Squirm = 1;
    [Header("Attack")]
    public float MeanTimeBetweenAttacks = 15f;
    public float AttackTimeVariance = 5f;
    public Transform Target;
    private bool _attacking = false;
    private float _noiseOffset = 0f;
    private float _attackTimer = 0f;
    private Vector3 _correction;

    private void Start()
    {
        InitializePath();
        Target = GameObject.FindObjectOfType<EthanPlayerMovement>().transform;
        _noiseOffset = Random.Range(0f, 100f);
        _attackTimer = MeanTimeBetweenAttacks + Random.Range(-AttackTimeVariance, AttackTimeVariance);
        GetComponent<Health>().CurrentHealth = 1;
    }

    private void Update()
    {
        UpdatePath();

        _attackTimer -= Time.deltaTime;
        if (_attackTimer < 0 && !_attacking)
            StartCoroutine(Attack());

        if (!_attacking)
        {
            Vector3 toTarget = Target.position - transform.position;
            Vector3 currentVelocity = transform.forward;

            // Desired velocity should be normal to a shpere centered at the target
            Vector3 desiredVelocity = Vector3.ProjectOnPlane(currentVelocity, Target.position - transform.position).normalized;
            // Set velocity maintain desired distance from target
            desiredVelocity += (toTarget.magnitude - DesiredDistance) * toTarget.normalized * CorrectionStrength;
            // Add some noisy movement perpendicular to the forward direction and towards the target
            desiredVelocity += Vector3.Cross(transform.forward, toTarget).normalized * (Mathf.PerlinNoise(Time.time + _noiseOffset, 0) * 2 - 1) * Squirm / 100;

            float floor = Mathf.Max(0, Target.position.y - DesiredHeight);
            float ceiling = Target.position.y + DesiredHeight;

            // add up velocity based on distance to floor and ceiling
            desiredVelocity += Vector3.up / Mathf.Pow(transform.position.y - floor, 2) * CorrectionStrength;
            desiredVelocity += Vector3.down / Mathf.Pow(transform.position.y - ceiling, 2) * CorrectionStrength;

            // Update position and rotation based on desired velocity
            transform.rotation = Quaternion.LookRotation(desiredVelocity, Vector3.up);
            transform.position += transform.forward * Speed * Time.deltaTime;
        }
    }

    private IEnumerator Attack()
    {
        _attacking = true;
        float normalSpeed = Speed;
        float attackSpeed = Speed * 4;

        // lerp to attack speed and rotate to face target
        Quaternion startRotation = transform.rotation;
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            SetSpeed(Mathf.Lerp(normalSpeed, attackSpeed, t));
            transform.rotation = Quaternion.Lerp(startRotation, Quaternion.LookRotation(Target.position - transform.position, Vector3.up), t);
            transform.position += transform.forward * Speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        
        // Move to the target
        Vector3 targetPosition = Target.position;
        Vector3 toTarget = targetPosition - transform.position;
        transform.rotation = Quaternion.LookRotation(toTarget, Vector3.up);

        while (Vector3.Distance(transform.position, targetPosition) > .1f)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            yield return null;
        }
        while (Vector3.Distance(transform.position, Target.position) < DesiredDistance)
        {
            transform.position += transform.forward * Speed * Time.deltaTime;
            yield return null;
        }

        _attacking = false;
        _attackTimer = MeanTimeBetweenAttacks + Random.Range(-AttackTimeVariance, AttackTimeVariance);
        
        yield return new WaitForSeconds(1f);

        // lerp back to prevSpeed]
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime;
            SetSpeed(Mathf.Lerp(attackSpeed, normalSpeed, t));
            yield return null;
        }
    }
}
