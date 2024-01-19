using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFire : MonoBehaviour
{
    [SerializeField] private Transform _gunTarget;
    [SerializeField] private Transform _gun;
    [SerializeField] private Animator _anim;
    [SerializeField] private float _fireNeededTime;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _projSpawn;
    [SerializeField] private Transform _reticle;
    [SerializeField] private AudioSource _fireAudio;
    [SerializeField] private Animator _reticleParentAnim;

    [SerializeField] private float _minSoundVolume = 0.2f;
    [SerializeField] private float _maxSoundVolume = 0.35f;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;
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
        if (GameManager.instance.PlayerDied) return;

        if (Input.GetMouseButton(1) && _canFire){
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
           //You can only fire every X seconds
            _fireCoolDownTime += Time.deltaTime;
            if(_fireCoolDownTime > _fireNeededTime)
            {
                FireProjectile();
            }

           
            //omly loops the sound if it has finished playing and we are still firing!
            if (!_fireAudio.isPlaying)
            {
                SoundManager();
            }
        }

        GunRotationUpdate();
    }

    void GunRotationUpdate()
    {
        //rotates the gun to slightly sway, following the player aiming 
        Vector3 relativeLocation = _gunTarget.position - _gun.position;
        _gun.rotation = Quaternion.LookRotation(relativeLocation, Vector3.up);
    }

    void BeginFiring()
    {
        Debug.Log("Fired!");
        _isFiring = true;
        _anim.SetBool("isFiring", true);
        _reticleParentAnim.SetBool("spin", true);
    }

    void StopFiring()
    {
        Debug.Log("Stopped Firing");
        _isFiring = false;
        _anim.SetBool("isFiring", false);
        _reticleParentAnim.SetBool("spin", false);
    }
   
    //What actually fires the projectile when _isFiring = true
    void FireProjectile()
    {
        Debug.Log("fired Projectile!");
        GameObject _spawnedProjectile = Instantiate(_projectile, _projSpawn.position, Quaternion.identity);
        //shooting the projectile "through" the middle of the screen reticle
        Ray _rayOrigin = Camera.main.ScreenPointToRay(_reticle.position);
        Vector3 shootDirection = _rayOrigin.direction;

        //need to correct angle so it shoot closer to the "Center" 
        Vector3 correctedAngle = _projSpawn.forward;
        correctedAngle = correctedAngle * 0.3f;
        shootDirection += correctedAngle;
        shootDirection = shootDirection.normalized;


        _spawnedProjectile.transform.rotation = Quaternion.LookRotation(shootDirection);
        _fireCoolDownTime = 0;
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
