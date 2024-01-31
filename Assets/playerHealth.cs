using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    [SerializeField] private TMP_Text _healthText;
    [SerializeField] private TMP_Text _percentageText;
    [SerializeField] private Animator _healthAnimator;
    [SerializeField] private AudioSource _deathAudio;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private Transform _shotGun;
    [SerializeField] private Transform _rifle;
    [SerializeField] private AudioSource _hurtSound;
    [SerializeField] private Animator _hurtScreenAnim;
    [SerializeField] private float _minKillDistance;
    [SerializeField] private EthanPlayerMovement movement;
    [SerializeField] private Animator _railgunAnim;
    [SerializeField] private Animator _rifleAnim;

    [SerializeField] private Animator _shotGunAnim;
    [SerializeField] private GameObject _railGun;
    [SerializeField] private WeaponFire _weaponFire;
    [SerializeField] private ShotGunFire _shotGunScript;
    [SerializeField] private float _inRageDrain;
    [SerializeField] private float _maxRage;

    [SerializeField] private Animator _ragePip1;
    [SerializeField] private Animator _ragePip2;
    [SerializeField] private Animator _ragePip3;

    public float _rage;
    public float _neededRage;
    public float _rageLossRate;
    public bool _canEnterRage = false;
    public bool _isRaging = false;
    public bool _addedRageRecent;
    public float recentRange;

    private Color _originalColor;
    private float _actualHealth;
    private float _displayedHealth;

    private void OnEnable()
    {
        _playerHealth.OnDamageTaken += DamageUpdateText;
        _playerHealth.OnDeath += OnDeathPlayer;
    }

    private void OnDisable()
    {
        _playerHealth.OnDamageTaken -= DamageUpdateText;
        _playerHealth.OnDeath -= OnDeathPlayer;
    }

    private void Start()
    {
        _actualHealth = _playerHealth.CurrentHealth;
        _healthText.text = _actualHealth.ToString();
        _originalColor = _healthText.color;
    }
    
    private void Update()
    {
        _displayedHealth = Mathf.Lerp(_displayedHealth, _actualHealth, Time.deltaTime * 5f);
        _healthText.text = (Mathf.RoundToInt(_displayedHealth).ToString() + "%");

        if(_playerHealth.CurrentHealth <= 5)
        {
            _hurtScreenAnim.SetTrigger("hurt");
        }

        RageUpdate();
       
        if(transform.position.y < _minKillDistance)
        {
            _playerHealth.TakeDamage(1000); 
        }
    }

    private void RageUpdate()
    {
       
        if (_isRaging)
        {
            _rage -= Time.deltaTime * _inRageDrain;

            if(_rage <= 0)
            {
                //ExitRage();
                _railgunAnim.SetBool("isFiring", false);
            }
        }
        else
        {
            if (_canEnterRage)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    EnterRage();
                }
            }

            if (_rage >= _neededRage)
            {
                _canEnterRage = true;
            }

            if (_addedRageRecent)
            {
                recentRange -= Time.deltaTime;
                if (recentRange <= 0)
                {
                    _addedRageRecent = false;
                }
            }
            else
            {
                _rage -= Time.deltaTime * _rageLossRate;
            }

        }


        _rage = Mathf.Clamp(_rage, 0, _maxRage);

        RageHudUpdate();

    }
    
    private void RageHudUpdate()
    {
       Image _fluidImage1 = _ragePip1.GetComponent<Image>();
       Image _fluidImage2 = _ragePip2.GetComponent<Image>();
       Image _fluidImage3 = _ragePip3.GetComponent<Image>();

       
        if(_rage >= 95f)
        {
           
            if(_rage >=  81f)
            {
                _ragePip1.SetInteger("rage", 2);
                _ragePip2.SetInteger("rage", 2);
                _ragePip3.SetInteger("rage", 2);
            }
            else
            {
            _ragePip1.SetInteger("rage", 2);
            _ragePip2.SetInteger("rage", 2);
            _ragePip3.SetInteger("rage", 1);


            }
            

        }
        else if(_rage >= 66f)
        {
            if(_rage >= 51f)
            {
                _ragePip1.SetInteger("rage", 2);
                _ragePip2.SetInteger("rage", 2);
                _ragePip3.SetInteger("rage", 0);

            }
            else
            {
                _ragePip1.SetInteger("rage", 2);
                _ragePip2.SetInteger("rage", 1);
                _ragePip3.SetInteger("rage", 0);
            }

        }
        else if(_rage >= 33f) 
        {

            _ragePip1.SetInteger("rage", 2);
            _ragePip2.SetInteger("rage", 0);
            _ragePip3.SetInteger("rage", 0);
        }
        else
        {
            if(_rage > 11f)
            {
                _ragePip1.SetInteger("rage", 1);
                _ragePip2.SetInteger("rage", 0);
                _ragePip3.SetInteger("rage", 0);
            }
            else
            {
                _ragePip1.SetInteger("rage", 0);
                _ragePip2.SetInteger("rage", 0);
                _ragePip3.SetInteger("rage", 0);

            }          
        }
   

        
        
    }

    public void AddRage(float _addedValue)
    {
       if(!_isRaging)
        {
        _rage += _addedValue;
        _addedRageRecent = true;
        recentRange = 2;

        }    
        
    }

    private void EnterRage()
    {
        Debug.Log("ENTERED RAGE");
        movement._turretMode = true;
        _isRaging = true;

        //disables two current guns and enables railgun
        _shotGunAnim.SetBool("active", false);
        _shotGunScript.enabled = false;
       
        _rifleAnim.SetBool("active", false);
        _weaponFire.enabled = false;

        _railGun.SetActive(true);
         
    }

    public void ExitRage()
    {
        _isRaging = false;
        movement._turretMode = false;

        _railGun.SetActive(false);

        _shotGunAnim.SetBool("active", true);
        _shotGunScript.enabled = true;

        _rifleAnim.SetBool("active", true);
        _weaponFire.enabled = true;

    }

    private void DamageUpdateText(float oldHealth, float newHealth)
    {  
        //Sets the health UI to reflect playerHealth
        _actualHealth = newHealth;
        StopAllCoroutines();
        StartCoroutine(FlashHealth());
        _healthAnimator.SetTrigger("hurt");
        _hurtScreenAnim.SetTrigger("hurt");
        _hurtSound.Play();
    }

    private void OnDeathPlayer()
    {
        //this is where the player will die!
        Debug.Log("playerDied!");
        if (_deathAudio.isPlaying == false)
        {
        _deathAudio.Play();
        }

        Debug.Log("Played Death Audio!");
        Transform _cam = Camera.main.transform;
        _cam.SetParent(null);
        _shotGun.SetParent(null);
        _rifle.SetParent(null);
        Rigidbody _rigid = _cam.gameObject.AddComponent<Rigidbody>();
        Vector3 _direction = new Vector3(10, 10, 10);
         _rigid.velocity = (Vector3.up * 2);
        _rigid.angularVelocity = Vector3.right * 1.0f;
        Destroy(transform.gameObject);
    }

    private IEnumerator FlashHealth()
    {
        _healthText.color = _originalColor;
        _percentageText.color = _originalColor;
        yield return new WaitForSeconds(0.1f);
        _healthText.color = Color.red;
        _percentageText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        _healthText.color = _originalColor;
        _percentageText.color = _originalColor;
    }
}
