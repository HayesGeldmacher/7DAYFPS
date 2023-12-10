using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private Transform _gunTarget;
    [SerializeField] private Transform _gun;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            Fire();
        }

        //GunRotationUpdate();
    }

    void GunRotationUpdate()
    {
         Vector3 relativeLocation = _gunTarget.position - _gun.position;
        _gun.rotation = Quaternion.LookRotation(relativeLocation, Vector3.up);
    }

    void Fire()
    {
        Debug.Log("Fired!");
    }
}
