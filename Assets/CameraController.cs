using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;
    [Header("Sensitivity")]
    [SerializeField] private float _mouseSensitivityX = 1f;
    [SerializeField] private float _mouseSensitivityY = 1f;
    [Header("Camera Tilt")]
    [SerializeField] private float _forwardCameraTilt = 1f;
    [SerializeField] private float _sideCameraTilt = 3f;
    [Header("FOV")]
    [SerializeField] private float _dashFOV = 60f;

    private Rigidbody _playerRb;
    private EthanPlayerMovement _playerMovement;
    private Camera _camera;
    private float _cameraVerticalRotation = 0f;

    private void Awake()
    {
        _playerRb = _playerBody.GetComponent<Rigidbody>();
        _playerMovement = _playerBody.GetComponent<EthanPlayerMovement>();
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Rotate the player left and right
        _playerBody.transform.Rotate(Vector3.up * mouseX * _mouseSensitivityX);

        // Rotate the camera up and down
        _cameraVerticalRotation -= mouseY * _mouseSensitivityY;
        _cameraVerticalRotation = Mathf.Clamp(_cameraVerticalRotation, -90, 90);

        // Determine the camera tilt based on the player's velocity
        float sideTiltAmount = Vector3.Dot(_playerMovement.Velocity, _playerBody.transform.right) / _playerMovement.MaxSpeed;
        float forwardTiltAmount = Vector3.Dot(_playerMovement.Velocity, _playerBody.transform.forward) / _playerMovement.MaxSpeed;
        
        // Set the camera's rotation
        _camera.transform.localRotation = Quaternion.Euler(_cameraVerticalRotation + forwardTiltAmount * _forwardCameraTilt, 0f, -sideTiltAmount * _sideCameraTilt);
        
        // Set the camera's FOV based on the player's dash state
        _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _playerMovement.Dashing ? 70 : 60, 10 * Time.deltaTime);
    }
}
