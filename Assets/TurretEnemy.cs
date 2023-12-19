using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    [SerializeField] private float _firingRange = 50f;
    [SerializeField] private float _firingSpeed = 50f;
    [SerializeField] private float _rotationSpeed = 50f;
    private bool _inRange = false;

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
        
    }

    private void RangeUpdate()
    {
       float _distance = Vector3.Distance(_player.position, transform.position);
        if(_distance < _firingRange)
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
        if(_inRange)
        {
            Vector3 _relativePos = _player.position - transform.position;

            // the second argument, upwards, defaults to Vector3.up
            Quaternion rotation = Quaternion.LookRotation(_relativePos, Vector3.up);
            transform.rotation = rotation;
        }
    }
}
