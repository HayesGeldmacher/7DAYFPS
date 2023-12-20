using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _fireLocation;
    [SerializeField] private float _firingRange = 50f;
    [SerializeField] private float _firingCoolDown = 4;
    [SerializeField] private float _firingDelay = 0.3f;
    [SerializeField] private float _rotationSpeed = 50f;
    [SerializeField] private float _damage = 15f;
    [SerializeField] private LayerMask _layerMask;
    [Header("Sound Attributes")]
    [SerializeField] private AudioSource _chargeUp;
    [SerializeField] private AudioSource _blast;
    [SerializeField] private bool _isBuilding;
    [SerializeField] private float _pitchMin;
    [SerializeField] private float _pitchMax;
    [SerializeField] private float _pitchSpeed;

    private Vector3 _fireDirection;
    private bool _inRange = false;
    private bool _canFire = false;

    private Transform _player;

    // Start is called before the first frame update
    void Start()
    {
        _player = EthanPlayerMovement.instance.transform;
        StartCoroutine(FireCoolDown()); 


    }

    // Update is called once per frame
    void Update()
    {
        RangeUpdate();
        RotationUpdate();
        FireUpdate();
        SoundUpdate();
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
        if( _inRange)
        {
            if (_canFire)
            {
                StartCoroutine(FireLaser(transform.forward));
                _isBuilding = false;
            }

            else
            {
                _isBuilding = true;
            }
        }
        else
        {
            _isBuilding = false;
        }
    }

    private IEnumerator FireLaser(Vector3 currenRotation)
    {
        _blast.Play();
        _canFire = false;
        _fireDirection = currenRotation;
        yield return new WaitForSeconds(_firingDelay);

        GameObject shotProjectile = Instantiate(_projectile, _fireLocation.position, Quaternion.identity);
        shotProjectile.transform.rotation = Quaternion.LookRotation(currenRotation);

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

    private void SoundUpdate()
    {
        if (_isBuilding)
        {
            if (_chargeUp.isPlaying)
            {
                _chargeUp.pitch += 1 * _pitchSpeed * Time.deltaTime;
            }
            else
            {
                _chargeUp.Play();
                _chargeUp.pitch = _pitchMin;
            }

            _chargeUp.pitch = Mathf.Clamp(_chargeUp.pitch, _pitchMin, _pitchMax);
            //continually up the pitch for as long as building!
        }
        else
        {
            _chargeUp.Stop();
            _chargeUp.pitch = _pitchMin;
        }
    }

}
