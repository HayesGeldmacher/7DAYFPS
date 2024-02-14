using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DealPlayerDamage : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _damageInterval = 1f;
    private float _damageTimer = 0f;
    [SerializeField] private bool _killSelfAttack;
    [SerializeField] private float _faceRestriction;


    private void Update()
    {
        // if (_damageTimer > 0)
        _damageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_damageTimer > 0) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        
        if (!other.GetComponent<EthanPlayerMovement>().isPounding)
        {
            float dot = Vector3.Dot(other.transform.forward, (transform.position - other.transform.position).normalized);
            Debug.Log("Dot + " + dot);
            if(dot > _faceRestriction)
            {
                other.GetComponent<Health>()?.TakeDamage(_damage);
                if (_killSelfAttack)
                {
                    Health health = transform.GetComponent<Health>();
                    health.TakeDamage(1000);
                }
                _damageTimer = _damageInterval;

            }

        }





    }

    private void OnTriggerStay(Collider other)
    {
        if (_damageTimer > 0) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        if (!other.GetComponent<EthanPlayerMovement>().isPounding)
        {
            float dot = Vector3.Dot(other.transform.forward, (transform.position - other.transform.position).normalized);
            Debug.Log("Dot + " + dot);
            if (dot > _faceRestriction)
            {
                other.GetComponent<Health>()?.TakeDamage(_damage);
                if (_killSelfAttack)
                {
                    Health health = transform.GetComponent<Health>();
                    health.TakeDamage(1000);
                }
                _damageTimer = _damageInterval;

            }

        }
    }

}
