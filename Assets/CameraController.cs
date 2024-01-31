using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _playerBody;
    [Header("Sensitivity")]
    public float _mouseSensitivityX = 1f;
    public float _mouseSensitivityY = 1f;
    [Header("Camera Tilt")]
    [SerializeField] private float _forwardCameraTilt = 1f;
    [SerializeField] private float _sideCameraTilt = 3f;
    [Header("FOV")]
    [SerializeField] private float _dashFOV = 60f;

    [Header("Reticle")]
     private float currentReticleX;
     private float currentReticleY;
    [SerializeField] private float maxReticleX;
    [SerializeField] private float maxReticleY;
    [SerializeField] private float reticleAcceleration;
    [SerializeField] private float reticleDecceleration;
    [SerializeField] private Transform Reticle;
    [SerializeField] private Transform gun;

    [Header("Gun")]
    private float currentGunX;
    private float currentGunY;
    [SerializeField] private float maxGunX;
    [SerializeField] private float maxGunY;
    [SerializeField] private float gunAcceleration;
    [SerializeField] private float gunDecceleration;  


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
        if (GameManager.instance.PlayerDied) return;

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

        ReticleSwayUpdate();
        GunSwayUpdate();
    }

        void ReticleSwayUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Normalize mouse input (Makes it move in a circle instead of a square)
        Vector2 mouseInput = Vector2.ClampMagnitude(new Vector2(mouseX, mouseY), 1);
        mouseX = mouseInput.x;
        mouseY = mouseInput.y;

        // Set target reticle position based on mouse input
        float targetReticleX = Mathf.Abs(mouseX) < 0.1f ? 0 : mouseX > 0 ? maxReticleX : -maxReticleX;
        float targetReticleY = Mathf.Abs(mouseY) < 0.1f ? 0 : mouseY > 0 ? maxReticleY : -maxReticleY;

        // // Set target reticle position based on player velocity
        // targetReticleX -= Vector3.Dot(_playerMovement.Velocity, _playerBody.transform.right) / _playerMovement.MaxSpeed * maxReticleX;
        // targetReticleY -= Vector3.Dot(_playerMovement.Velocity, _playerBody.transform.up) / _playerMovement.MaxSpeed * maxReticleY;
        
        // Lerp current reticle position towards target reticle position
        currentReticleX = Mathf.Lerp(currentReticleX, targetReticleX, reticleAcceleration * Time.deltaTime);
        currentReticleY = Mathf.Lerp(currentReticleY, targetReticleY, reticleAcceleration * Time.deltaTime);

        // Set reticle position
        Vector3 cursorPos = new Vector3(currentReticleX, currentReticleY, Reticle.localPosition.z);
        Reticle.localPosition = cursorPos;
    }

    void GunSwayUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Normalize mouse input (Makes it move in a circle instead of a square)
        Vector2 mouseInput = Vector2.ClampMagnitude(new Vector2(mouseX, mouseY), 1);
        mouseX = mouseInput.x;
        mouseY = mouseInput.y;

        // Set target gun position based on mouse input
        float targetGunX = Mathf.Abs(mouseX) < 0.1f ? 0 : mouseX > 0 ? maxGunX : -maxGunX;
        float targetGunY = Mathf.Abs(mouseY) < 0.1f ? 0 : mouseY > 0 ? maxGunY : -maxGunY;

        // Set target gun position based on player velocity
        targetGunX -= Vector3.Dot(_playerMovement.Velocity, _playerBody.transform.right) / _playerMovement.MaxSpeed * maxGunX;
        targetGunY -= Vector3.Dot(_playerMovement.Velocity, _playerBody.transform.up) / _playerMovement.MaxSpeed * maxGunY;
        
        // Lerp current gun position towards target gun position
        currentGunX = Mathf.Lerp(currentGunX, targetGunX, reticleAcceleration * Time.deltaTime);
        currentGunY = Mathf.Lerp(currentGunY, targetGunY, reticleAcceleration * Time.deltaTime);

        // Set gun position
        Vector3 cursorPos = new Vector3(currentGunX, currentGunY, gun.localPosition.z);
        gun.localPosition = cursorPos;
    }
}

    

