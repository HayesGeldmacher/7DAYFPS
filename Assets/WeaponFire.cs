using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private Transform _gunTarget;
    [SerializeField] private Transform _gun;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _fireNeededTime;
    private float _fireCoolDownTime;
    private bool _isFiring = false;
    private bool _canFire = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && _canFire){
            if (!_isFiring)
            {
                BeginFiring();
            }
        }
        else
        {
            if (_isFiring)
            {
                StopFiring();
            }
        }

        if (_isFiring)
        {
            _fireCoolDownTime += Time.deltaTime;
            if(_fireCoolDownTime > _fireNeededTime)
            {
                FireProjectile();
            }
        }

        GunRotationUpdate();
    }

    void GunRotationUpdate()
    {
         Vector3 relativeLocation = _gunTarget.position - _gun.position;
        _gun.rotation = Quaternion.LookRotation(relativeLocation, Vector3.up);
    }

    void BeginFiring()
    {
        Debug.Log("Fired!");
        _isFiring = true;
        _anim.SetBool("isFiring", true);
    }

    void StopFiring()
    {
        Debug.Log("Stopped Firing");
        _isFiring = false;
        _anim.SetBool("isFiring", false);
    }

    void FireProjectile()
    {
        Debug.Log("fired Projectile!");
        //fire 
        _fireCoolDownTime = 0;
    }
}
