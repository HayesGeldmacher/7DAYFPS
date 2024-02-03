using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(CharacterController))]
public class EthanPlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] public float MaxSpeed = 10f;
    [SerializeField] private float _acceleration = 10f;
    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashAcceleration = 20f;
    [SerializeField] private float _dashDuration = .25f;
    [SerializeField] private float _dashCooldown = .25f;
    [SerializeField] private AudioSource _dashSound;
    [Header("Jump")]
    [SerializeField] private float _jumpVelocity = 10f;
    [SerializeField] private float _boostVelocity = 10f;
    [SerializeField] private float _boostAcceleration = 10f;
    [Range(0, 1)][SerializeField] private float _airControl = 0.2f;
    [SerializeField] private float _jumpCooldown = 0.1f;
    [Header("Energy")]
    [SerializeField] private float _maxEnergy = 100f;
    [SerializeField] private float _currentEnergy = 100f;
    [SerializeField] private float _energyCostDash = 8;
    [SerializeField] private float _energyCostIdle = 1;
    [SerializeField] private float _energyCostBoost = 4;
    [SerializeField] private TMP_Text _energyText;
    [Header("Audio")]
    [SerializeField] private float _minSoundVolume = 0.8f;
    [SerializeField] private float _maxSoundVolume = 0.12f;
    [SerializeField] private float _minPitch = 0.8f;
    [SerializeField] private float _maxPitch = 1.2f;
    [SerializeField] private CameraController _camControl;
    [SerializeField] private float _chargedJumpHeight;
    [SerializeField] private float _jumpHoldTime;
    [SerializeField] private float _currentJumpHoldTime;
    [SerializeField] private bool _chargedJump = false;
    [SerializeField] private Animator _springAnim;
    public bool _turretMode = false;
    [SerializeField] private Transform _cam;
    [SerializeField] private Transform _camCrouch;
    [SerializeField] private Transform _camOrigin;
    [SerializeField] private AudioSource _jumpAudio;
    [SerializeField] private float _jetPackStrength;
    [SerializeField] private GameObject _shockWave;
    [SerializeField] private LayerMask _groundedMask;

    private float _storedCamX;
    private float _storedCamY;


    public Vector3 Velocity = Vector3.zero;
    public bool Grounded = false;
    public bool Dashing = false;
    private bool Boosting = false;
    private bool _canDash = false;

    private Vector3 _dashDirection = Vector3.zero;
    private float _dashTimer = 0f;
    private float _jumpTimer = 0f;
    private CharacterController _controller;

    [HideInInspector] public bool isPounding = false;
    public float _groundPoundDamage;
    [SerializeField] private float _groundPoundRadius;
    private float timeSinceJumped;

    #region Singleton

    public static EthanPlayerMovement instance;

    #endregion


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of inventory present!! NOT GOOD!");
            return;
        }

        instance = this;
    }

    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
        _currentJumpHoldTime = 0;
        timeSinceJumped = 0;

        _storedCamX = _camControl._mouseSensitivityX;
        _storedCamY = _camControl._mouseSensitivityY;
    }

    private void Update()
    {
        if (GameManager.instance.PlayerDied) return;

        if (_camControl._mouseSensitivityX != _storedCamX)
        {
            _camControl._mouseSensitivityX = _storedCamX;
        }
        if (_camControl._mouseSensitivityY != _storedCamY)
        {
            _camControl._mouseSensitivityY = _storedCamY;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Check if the player is grounded
        Grounded = Physics.Raycast(transform.position, Vector3.down, 1.5f, _groundedMask);

        // Check for jump and dash
        _dashTimer -= Time.deltaTime;
        _jumpTimer -= Time.deltaTime;
        Boosting = false;
        if (Input.GetKey(KeyCode.Space))
        {
            Boosting = true;

            if (Input.GetKeyDown(KeyCode.Space) && _jumpTimer <= 0 && Grounded)
            {
                // Jump(false);


            }
            else if (Input.GetKey(KeyCode.Space))
            {
                Boost();
            }

            if (!Grounded)
            {
                JetPackFly();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (_chargedJump)
            {
                Jump(true);
            }
            else
            {
                Jump(false);
            }

            ExitBoost();
        }



        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTimer <= 0)
        {
            StartCoroutine(Dash());
        }

        // Update velocity from input
        Vector3 desiredVelocity;
        float availableAcceleration;
        if (Dashing)
        {
            desiredVelocity = _dashDirection * _dashSpeed;
            availableAcceleration = _dashAcceleration;
        }
        else if (!Boosting)
        {
            desiredVelocity = (transform.forward * vertical + transform.right * horizontal).normalized * MaxSpeed;
            availableAcceleration = Grounded ? _acceleration : _acceleration * _airControl;
        }
        else
        {
            desiredVelocity = (transform.forward * vertical + transform.right * horizontal).normalized * (MaxSpeed * 0.7f);
            availableAcceleration = Grounded ? _acceleration : _acceleration * _airControl;
        }
        Vector3 actualVelocity = Vector3.ProjectOnPlane(Velocity, Vector3.up);
        Velocity += (desiredVelocity - actualVelocity) * availableAcceleration * Time.deltaTime;



        // Gravity
        if (!isPounding)
        {
        Velocity += Vector3.up * Physics.gravity.y * 2.5f * Time.deltaTime;
        }
        else
        {
            Velocity += Vector3.up * -400 * Time.deltaTime;
        }
        if (Dashing || (Grounded && Velocity.y < 0))
            Velocity.y = 0;


        if(!Grounded)
        {
            timeSinceJumped += Time.deltaTime;
            
            if(!isPounding && Input.GetKey(KeyCode.LeftControl))
            {
               if(timeSinceJumped > 0.8f && transform.position.y >= 8)
                {
                
                EnterGroundPound();

                }
            }    
        }
        else
        {
            if (isPounding)
            {
            isPounding = false;
                ShockWave();
            }
            timeSinceJumped = 0;
        }


        if (Boosting)
        {
            if (Velocity.x < -0.1f || Velocity.x > 0.1f)
            {
                Velocity.x = Mathf.Lerp(Velocity.x, 0, 6 * Time.deltaTime);
            }

            if (Velocity.z > 0.1f || Velocity.z < -0.1f)
            {
                Velocity.z = Mathf.Lerp(Velocity.z, 0, 6 * Time.deltaTime);
            }

            float _distance = Vector3.Distance(_cam.position, _camCrouch.position);
            if (_distance > 0.1f)
            {
                _cam.position = Vector3.Lerp(_cam.position, _camCrouch.position, 3 * Time.deltaTime);
            }

        }
        else
        {
            float _distance = Vector3.Distance(_cam.position, _camOrigin.position);
            if (_distance > 0.1f)
            {
                _cam.position = Vector3.Lerp(_cam.position, _camOrigin.position, 8 * Time.deltaTime);
            }
        }

        _controller.Move(Velocity * Time.deltaTime);

        //drains energy every fram based on the current state of the player
        CheckEnergyUpdate();
    }

    private void CheckEnergyUpdate()
    {

        float boostConsumptionRate = Dashing ? _energyCostDash : Boosting ? _energyCostBoost : _energyCostIdle;
        _currentEnergy -= boostConsumptionRate * Time.deltaTime;

        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        _energyText.text = ((int)_currentEnergy).ToString();
    }

    private void Jump(bool isCharged)
    {
        float jumpHeight;
        if (isCharged)
        {
            jumpHeight = _chargedJumpHeight;
            _jumpAudio.Play();
            _springAnim.SetTrigger("spring");
            _springAnim.SetBool("charging", false);
        }
        else
        {
            jumpHeight = _jumpVelocity;
        }

        Velocity.y += jumpHeight;
        _jumpTimer = _jumpCooldown;
    }

    private void JetPackFly()
    {
       
        Velocity += Vector3.up * _jetPackStrength * Time.deltaTime;
        _currentEnergy -= _energyCostBoost * Time.deltaTime; 
        
    }


    private void Boost()
    {
        if (Grounded)
        {    
            _currentJumpHoldTime += Time.deltaTime;       
            Boosting = true;
            _springAnim.SetBool("charging", true);    

        }
            else
            {
                ExitBoost();
            }

            if (_currentJumpHoldTime >= _jumpHoldTime)
            {
                _chargedJump = true;
            }      

    }
        private void ExitBoost()
        {

            _springAnim.SetBool("charging", false);
            _springAnim.ResetTrigger("spring");
            _currentJumpHoldTime = 0;
            _chargedJump = false;
            Boosting = false;
        }

        private void EnterGroundPound()
        {
            isPounding = true;
            Velocity += Vector3.up * -10;
        }

        private IEnumerator Dash()
        {
            //if (_currentEnergy < _energyCostDash * _dashDuration) yield break;

            SoundManager(_dashSound);
            Dashing = true;
            Vector3 input = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
            if (input.magnitude > .1f)
                _dashDirection = input.normalized;
            else if (Vector3.ProjectOnPlane(Velocity, Vector3.up).magnitude > MaxSpeed / 2)
                _dashDirection = Vector3.ProjectOnPlane(Velocity, Vector3.up).normalized;
            else
                _dashDirection = transform.forward;
            yield return new WaitForSeconds(_dashDuration);
            Dashing = false;
            _dashTimer = _dashCooldown;
        }

        private IEnumerator Transform(float time)
        {
            //this is where the player will transform from aerial to turret mode!
            yield return new WaitForSeconds(time);
            Debug.Log("Transformed");
        }



        private void SoundManager(AudioSource _audio)
        {
            //randomly play sound at different pitch and volume


            float randomPitch = Random.Range(_minPitch, _maxPitch);
            float randomVolume = Random.Range(_minSoundVolume, _maxSoundVolume);

            _audio.pitch = randomPitch;
            _audio.volume = randomVolume;
            _audio.Play();
        }
    

    private void ShockWave()
    {
        GameObject shockwave = Instantiate(_shockWave, transform.position, Quaternion.identity);


    }

    private void OnCollisionEnter(Collision other)
    {
        if (isPounding)
        {
            if(other.transform.tag == "enemy")
            {
                other.gameObject.GetComponent<Health>()?.TakeDamage(500);
            }
        }
    }
}
