using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine.UIElements;

[RequireComponent(typeof(Rigidbody))]
public class EthanPlayerMovement : MonoBehaviour
{
    [Header("Basic Movement")]
    [SerializeField] private float _maxSpeed = 10f;
    [SerializeField] private float _acceleration = 10f;
    [Header("Dash")]
    [SerializeField] private float _dashSpeed = 20f;
    [SerializeField] private float _dashAcceleration = 20f;
    [SerializeField] private float _dashDuration = .25f;
    [SerializeField] private float _dashCooldown = .25f;
    [Header("Jump")]
    [SerializeField] private float _jumpForce = 10f;
    [Range(0,1)] [SerializeField] private float _airControl = 0.2f;
    [SerializeField] private float _jumpCooldown = 0.1f;
    [Header("Sensitivity")]
    [SerializeField] private float _mouseSensitivityX = 1f;
    [SerializeField] private float _mouseSensitivityY = 1f;
    [Header("Camera")]
    [SerializeField] private float _forwardCameraTilt = 1f;
    [SerializeField] private float _sideCameraTilt = 3f;

    private Rigidbody _rb;
    private Camera _camera;
    private float _cameraVerticalRotation = 0f;
    private bool _grounded = false;
    private bool _dashing = false;
    private Vector3 _dashDirection = Vector3.zero;
    private float _dashTimer = 0f;
    private float _jumpTimer = 0f;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Move the player
        Vector3 desiredVelocity;
        float availableAcceleration;
        if (_dashing)
        {
            desiredVelocity = _dashDirection * _dashSpeed;
            availableAcceleration = _dashAcceleration;
        }
        else
        {
            desiredVelocity = (transform.forward * vertical + transform.right * horizontal).normalized * _maxSpeed;
            availableAcceleration = _grounded ? _acceleration : _acceleration * _airControl;
        }
        Vector3 actualVelocity = Vector3.ProjectOnPlane(_rb.velocity, Vector3.up);
        _rb.AddForce((desiredVelocity - actualVelocity) * availableAcceleration, ForceMode.Acceleration);
        
        // Apply gravity
        if (_rb.velocity.y < 0 || _rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            _rb.AddForce(Vector3.up * Physics.gravity.y * 4, ForceMode.Acceleration);
        else
            _rb.AddForce(Vector3.up * Physics.gravity.y * 2, ForceMode.Acceleration);
        if (_dashing)
            _rb.velocity = Vector3.ProjectOnPlane(_rb.velocity, Vector3.up);

    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Check if the player is grounded
        _grounded = Physics.Raycast(transform.position, Vector3.down, 1.25f);

        // Rotate the player and the camera
        transform.Rotate(Vector3.up * mouseX * _mouseSensitivityX);
        _cameraVerticalRotation -= mouseY * _mouseSensitivityY;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90, 90);
        float sideTiltAmount = Vector3.Dot(_rb.velocity, transform.right) / _maxSpeed;
        Debug.Log(sideTiltAmount);
        float forwardTiltAmount = Vector3.Dot(_rb.velocity, transform.forward) / _maxSpeed;
        _camera.transform.localRotation = Quaternion.Euler(_cameraVerticalRotation + forwardTiltAmount * _forwardCameraTilt, 0f, -sideTiltAmount * _sideCameraTilt);
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _dashing ? 70 : 60, 10 * Time.deltaTime);

        // Check for jump and dash
        _dashTimer -= Time.deltaTime;
        _jumpTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space) && _jumpTimer <= 0 && _grounded)
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && _dashTimer <= 0)
        {
            StartCoroutine(Dash());
        }
    }

    private void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        _jumpTimer = _jumpCooldown;
    }

    private IEnumerator Dash()
    {
        _dashing = true;
        Vector3 input = transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal");
        _dashDirection = input.magnitude > .1f ? input.normalized : transform.forward;
        yield return new WaitForSeconds(_dashDuration);
        _dashing = false;
        _dashTimer = _dashCooldown;
    }
}
