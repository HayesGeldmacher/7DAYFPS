using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private Transform _gunTarget;
    [SerializeField] private Transform _gun;
    [SerializeField] private Animator _anim;
    private bool _isFiring = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0)){
            FireProjectile();
        }
        else
        {
            
        }

        GunRotationUpdate();
    }

    void GunRotationUpdate()
    {
         Vector3 relativeLocation = _gunTarget.position - _gun.position;
        _gun.rotation = Quaternion.LookRotation(relativeLocation, Vector3.up);
    }

    void FireProjectile()
    {
        Debug.Log("Fired!");
        _isFiring=true;
        _anim.SetTrigger("Fire");
    }
}
