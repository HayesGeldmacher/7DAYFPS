using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DealPlayerDamage : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _damageInterval = 1f;
    private float _damageTimer = 0f;

    private void Update()
    {
        // if (_damageTimer > 0)
        _damageTimer -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_damageTimer > 0) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        other.GetComponent<Health>()?.TakeDamage(_damage);

        _damageTimer = _damageInterval;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_damageTimer > 0) return;
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;

        other.GetComponent<Health>()?.TakeDamage(_damage);

        _damageTimer = _damageInterval;
    }

}
