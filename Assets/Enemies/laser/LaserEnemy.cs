using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private float _firingRange = 50f;
    [SerializeField] private float _firingCoolDown = 4;
    [SerializeField] private float _firingDelay = 0.3f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _damage = 15f;
    [SerializeField] private LayerMask _layerMask;
    private Vector3 _fireDirection;
    private bool _inRange = false;
    private bool _canFire = true;

    private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = EthanPlayerMovement.instance.transform;

    }

    // Update is called once per frame
    void Update()
    {
        RangeUpdate();
        RotationUpdate();
        FireUpdate();
    }

    private void RangeUpdate()
    {
        float _distance = Vector3.Distance(_player.position, transform.position);
        if (_distance < _firingRange)
        {
            _inRange = true;
        }
        else
        {
            _inRange = false;
        }
    }

    private void RotationUpdate()
    {
        if (_inRange)
        {
            Vector3 _relativePos = _player.position - transform.position;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion _rotation = Quaternion.LookRotation(_relativePos, Vector3.up);
            transform.rotation = _rotation;
  
        }
    }

    private void FireUpdate()
    {   
        if( _inRange && _canFire)
        {
            StartCoroutine(FireLaser(transform.forward));
        }
    }

    private IEnumerator FireLaser(Vector3 currenRotation)
    {
        _canFire = false;
        _fireDirection = currenRotation;
        yield return new WaitForSeconds(_firingDelay);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, _fireDirection, out hit, _firingRange, _layerMask))
        {
            Debug.DrawRay(transform.position, _fireDirection * 10000, Color.yellow, 3);
            Debug.Log("Did Hit");
            DealDamage();
        }
        else
        {
            Debug.DrawRay(transform.position, _fireDirection * 10000, Color.red, 3);
        }

        StartCoroutine(FireCoolDown());
        Debug.Log("FIRED~~~");
    }


    private IEnumerator FireCoolDown()
    {
        yield return new WaitForSeconds(_firingCoolDown);
        _canFire = true;
    }


    private void DealDamage()
    {
        _player.GetComponent<Health>()?.TakeDamage(_damage);
    }

}
