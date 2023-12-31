using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEnemy : MonoBehaviour
{
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _fireLocation;
    [SerializeField] private float _firingRange = 50f;
    [SerializeField] private float _minFiringCoolDown = 4;
    [SerializeField] private float _maxFiringCoolDown = 9;
    [SerializeField] private float _firingDelay = 0.3f;
    [SerializeField] private float _buildUpTime = 0.3f;
    [SerializeField] private float _damage = 15f;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Animator _anim;
    private float _waitTime = 3;
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
                StartCoroutine(FireLaser());
            }
        }
        else
        {
            _isBuilding = false;
            _anim.SetBool("isCharging", false);
        }
    }

    private IEnumerator FireLaser()
    {
        _canFire = false;
        _isBuilding = true;
        _anim.SetBool("isCharging", true);
        yield return new WaitForSeconds(_buildUpTime);
        _fireDirection = transform.forward;
        yield return new WaitForSeconds(_firingDelay);
        if (_inRange)
        {
            BlastSound();
        }
        _isBuilding = false;
        _anim.SetTrigger("fire");
        _anim.SetBool("isCharging", false);

        GameObject shotProjectile = Instantiate(_projectile, _fireLocation.position, Quaternion.identity);
        shotProjectile.transform.rotation = Quaternion.LookRotation(_fireDirection);

        StartCoroutine(FireCoolDown());
        Debug.Log("FIRED~~~");
    }

    private IEnumerator FireCoolDown()
    {
         _waitTime = Random.Range(_minFiringCoolDown, _maxFiringCoolDown);
        yield return new WaitForSeconds(_waitTime);
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

    private void BlastSound()
    {
        _blast.pitch = Random.Range(0.8f, 1.2f);
        _blast.Play();
    }
}
