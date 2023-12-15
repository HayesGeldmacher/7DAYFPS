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
    [Header("Jump")]
    [SerializeField] private float _jumpVelocity = 10f;
    [SerializeField] private float _boostVelocity = 10f;
    [SerializeField] private float _boostAcceleration = 10f;
    [Range(0,1)] [SerializeField] private float _airControl = 0.2f;
    [SerializeField] private float _jumpCooldown = 0.1f;
    [Header("Energy")]
    [SerializeField] private float _maxEnergy = 100f;
    [SerializeField] private float _currentEnergy = 100f;
    [SerializeField] private float _energyCostDash = 8;
    [SerializeField] private float _energyBoostSpeed = 1;
    [SerializeField] private float _energyCostIdle = 1;
    [SerializeField] private float _energyIdleSpeed = 1;
    [SerializeField] private TMP_Text  _energyText;
    private float _idleEnergyTimer = 0;



    public Vector3 Velocity = Vector3.zero;
    public bool Grounded = false;
     public bool Dashing = false;
    private Vector3 _dashDirection = Vector3.zero;
    private float _dashTimer = 0f;
    private float _jumpTimer = 0f;
    private CharacterController _controller;
    private bool _isBoosting = false;


    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Check if the player is grounded
        Grounded = Physics.Raycast(transform.position, Vector3.down, 1.5f);

        // Check for jump and dash
        _dashTimer -= Time.deltaTime;
        _jumpTimer -= Time.deltaTime;
        if (Input.GetKey(KeyCode.Space))
        {
            if (Input.GetKeyDown(KeyCode.Space) && _jumpTimer <= 0 && Grounded)
            {
                Jump();
                _isBoosting = false;
            }
            else
            {
                Boost();
                _isBoosting = true;
            }
        }
        else
        {
            _isBoosting= false;
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
        else
        {
            desiredVelocity = (transform.forward * vertical + transform.right * horizontal).normalized * MaxSpeed;
            availableAcceleration = Grounded ? _acceleration : _acceleration * _airControl;
        }
        Vector3 actualVelocity = Vector3.ProjectOnPlane(Velocity, Vector3.up);
        Velocity += (desiredVelocity - actualVelocity) * availableAcceleration * Time.deltaTime;

        // Gravity
        Velocity += Vector3.up * Physics.gravity.y * 2.5f * Time.deltaTime;
        if (Dashing || (Grounded && Velocity.y < 0))
            Velocity.y = 0;

        _controller.Move(Velocity * Time.deltaTime);

       //drains idle energy every frame, then checks how much energy is left
        CheckEnergyUpdate();
    }

    private void CheckEnergyUpdate()
    {
        _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
        _energyText.text = _currentEnergy.ToString();
        
        if(_currentEnergy <= 0)
        {
            float time = 0;
            Transform(time);
        }

        //drains a constant small amount of energy from the player every second
        //Energy will drain faster if you are boosting in the air
        if (_isBoosting)
        {
            _idleEnergyTimer += Time.deltaTime * _energyBoostSpeed;
        }
        else
        {
            _idleEnergyTimer += Time.deltaTime * _energyIdleSpeed;
        }
        if( _idleEnergyTimer >= 1 ) {
            _idleEnergyTimer = 0;
            DrainEnergy(_energyCostIdle);
        }
    }

    private void Jump()
    {
        Velocity.y += _jumpVelocity;
        _jumpTimer = _jumpCooldown;
    }

    private void Boost()
    {
        Velocity.y += _boostAcceleration * Time.deltaTime;
        Velocity.y = Mathf.Clamp(Velocity.y, float.NegativeInfinity, _boostVelocity);
    }

    private IEnumerator Dash()
    {
        //drains energy to dash!
        DrainEnergy(_energyCostDash);

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

    private void DrainEnergy(float energyCost)
    {
        _currentEnergy -= energyCost;
    }
}
