using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotGunFire : MonoBehaviour
{
    [SerializeField] private Transform _gunTarget;
    [SerializeField] private Transform _gun;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _fireNeededTime;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _projSpawn;
    [SerializeField] private Transform _reticle;
    [SerializeField] private int _minShells;
    [SerializeField] private int _maxShells;
    [SerializeField] private float _shellAngleVariation;
    [SerializeField] private AudioSource _fireAudio;
    [SerializeField] private float _minSoundVolume = 0.2f;
    [SerializeField] private float _maxSoundVolume = 0.35f;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;
    [SerializeField] private Animator _cursorAnim;

    private float _fireCoolDownTime;
    private bool _canFire = true;

    // Update is called once per frame
    void Update()
    {
        if (!_canFire)
        {
            _fireCoolDownTime += Time.deltaTime; 
            if(_fireCoolDownTime >  _fireNeededTime)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    _canFire = true;
                }
            }
        }
        
        if (Input.GetMouseButton(0) && _canFire)
        {
            FireProjectile();
        }
    }

    void FireProjectile()
    {
       
        _anim.SetTrigger("fire");
        _cursorAnim.SetTrigger("fire");
        SoundManager();
       
        //slightly randomizing the number of shells that will spawn each fire
        int _shellsToFire = Random.Range(_minShells, _maxShells);
        Debug.Log(_shellsToFire);

       
        //for loop spawns and adds random direction to each individual shell
        for(int i = 0; i < _shellsToFire; i++ ){

            GameObject _spawnedProjectile = Instantiate(_projectile, _projSpawn.position, Quaternion.identity);

            Ray _rayOrigin = Camera.main.ScreenPointToRay(_reticle.position);
            Vector3 shootDirection = _rayOrigin.direction;

            float xVariation = Random.Range(-_shellAngleVariation, _shellAngleVariation);
            float yVariation = Random.Range(-_shellAngleVariation, _shellAngleVariation);
            float zVariation = Random.Range(-_shellAngleVariation, _shellAngleVariation);

            shootDirection = new Vector3(shootDirection.x + xVariation, shootDirection.y + yVariation, shootDirection.z + zVariation);

            //need to correct angle so it shoot closer to the "Center" 
            Vector3 correctedAngle = _projSpawn.forward;
            correctedAngle = correctedAngle * 0.3f;
            shootDirection += correctedAngle;


            shootDirection = shootDirection.normalized;
            _spawnedProjectile.transform.rotation = Quaternion.LookRotation(shootDirection);
        }

        _fireCoolDownTime = 0;
        _canFire = false;
    }

    void SoundManager()
    {
        //randomly play the shotgun audio at a different sound and pitch
        
        float randomPitch = Random.Range(_minPitch, _maxPitch);
        float randomVolume = Random.Range(_minSoundVolume, _maxSoundVolume);

        _fireAudio.pitch = randomPitch;
        _fireAudio.volume = randomVolume;
        _fireAudio.Play();
    }
}
