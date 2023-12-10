using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 Velocity = Vector3.zero;
    public bool Grounded = false;
     public bool Dashing = false;
    private Vector3 _dashDirection = Vector3.zero;
    private float _dashTimer = 0f;
    private float _jumpTimer = 0f;
    private CharacterController _controller;


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
                Jump();
            else
                Boost();
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
}
