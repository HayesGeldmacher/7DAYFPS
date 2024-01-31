using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Slider slider;

    [SerializeField] private float _energyLimit;
    [SerializeField] private float _currentEnergy;
    [SerializeField] private float _overHeatTime;
    [SerializeField] private AudioSource _reload;
    [SerializeField] private GameObject _chains;
    [SerializeField] private Animator _chainsAnim;

    [SerializeField] private Animator _flashAnim;

    private float _fireCoolDownTime;
    private bool _canFire = true;
    private bool _overHeating = false;

    
    void Start()
    {
        slider.maxValue = _energyLimit;
        slider.minValue = 0;

    }
    
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
        
        if (Input.GetMouseButton(0) && _canFire && !_overHeating)
        {
           if(_currentEnergy <= _energyLimit)
            {
                FireProjectile();
                _currentEnergy += 1;
            }
            else
            {
                StartCoroutine(OverHeat());
            }
           
        }

        _currentEnergy -= Time.deltaTime * 0.7f;
        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, 100);

        if (_overHeating)
        {
            slider.value = slider.maxValue;
            _anim.SetBool("overHeating", true);
        }
        else
        {

            slider.value = Mathf.Lerp(slider.value, _currentEnergy, Time.deltaTime * 5);
            _anim.SetBool("overHeating", false);

        }
    }

    private IEnumerator OverHeat()
    {
        _overHeating = true;
        _chains.SetActive(true);
        _chainsAnim.SetTrigger("fadeIn");
        _reload.Play();
        yield return new WaitForSeconds(_overHeatTime);
        _chainsAnim.SetTrigger("fadeOut");
       // _chains.SetActive(false);
        _overHeating = false;

    }
    
    
    
    void FireProjectile()
    {

        StartCoroutine(Flash());
        _anim.SetTrigger("fire");
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

    private IEnumerator Flash()
    {
        _flashAnim.SetBool("flashing", true);
        yield return new WaitForSeconds(0.1f);
        _flashAnim.SetBool("flashing", false);
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
