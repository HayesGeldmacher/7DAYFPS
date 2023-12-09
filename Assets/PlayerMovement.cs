using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    public Transform Camera;

    [Header("Speed Attributes")]
    public float currentSpeed;
    public float maxSpeed;
    public float speedAcceleration;

    public float maxBoostSpeed;
    public float boostSpeedAcceleration;

    public float maxDashSpeed;
    public float DashAcceleration;
    Vector3 driftingMove;

    Vector3 dashDirection;

    bool isMoving = false;
    
    //checks for the initial burst of speed after starting a boost
    bool hasDashed = false;

    //checks for the continued boost after dashing
    bool isBoosting = false;

    //Checks if the player can initiate a boost
    bool canBoost = true;

    public float slowDownAcceleration;

    [Header("Camera Attributes")]
    public float yCamSpeed;
     float defaultFOV;
     float zoomFOV;
     float backZoomFOV;

    
    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        //Setting stuff up for the camera FOV lerp
        Camera cam = Camera.GetComponent<Camera>();
        defaultFOV = cam.fieldOfView;
        zoomFOV = defaultFOV - 4;
        backZoomFOV = defaultFOV + 4;
        
    }

    
    void Update()
    {
        BoostUpdate();
        SetSpeedUpdate();
        MovementUpdate();
    }

    void BoostUpdate()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canBoost)
            {

                if (!hasDashed)
                {
                    StartCoroutine(Dash(0.1f));
                }

                isBoosting = true;
            }
            else
            {
                isBoosting = false;
                hasDashed = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isBoosting = false;
        }
        

    }

    void MovementUpdate()
    {
        //takes the raw player input to move character 
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        //Stores that input in a variable to be used later in function
        Vector3 move = (transform.right * x + transform.forward * y).normalized;

             

        if((Mathf.Abs(move.magnitude)) > 0.3f) 
        { 
            isMoving = true;
            driftingMove = (transform.right * x + transform.forward * y).normalized;
        }
        else
        {
            isMoving = false;
            
        }


        //What actually moves the character each frame
        if (hasDashed)
        {
            controller.Move(dashDirection * currentSpeed * Time.deltaTime);
        }
        else
        {

            if (isMoving)
            {
                controller.Move(move * currentSpeed * Time.deltaTime);
            }
            else
            {
                controller.Move(driftingMove * currentSpeed * Time.deltaTime);
            }
        }

        SetCameraFOV(y);
    }

    void SetSpeedUpdate()
    {
        float targetSpeed;
        float accelerationSpeed;

        if (hasDashed)
        {
            targetSpeed = maxDashSpeed;
            accelerationSpeed = DashAcceleration;
        }
        else if (isBoosting)
        {
            targetSpeed = maxBoostSpeed;
            accelerationSpeed = boostSpeedAcceleration;
        }
        else if (isMoving)
        {
            targetSpeed = maxSpeed;
            accelerationSpeed = speedAcceleration;
        }
        else
        {
            targetSpeed = 0;
            accelerationSpeed = slowDownAcceleration ;
        }
        
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, accelerationSpeed * Time.deltaTime);
    }

    void SetCameraFOV(float y)
    {
        //Lerps camera from side to side smoothly as player moves
        Camera cam = Camera.GetComponent<Camera>();
        if (y > 0.1f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, zoomFOV, yCamSpeed * Time.deltaTime);
        }
        else if (y < -0.1f)
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, backZoomFOV, yCamSpeed * Time.deltaTime);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, defaultFOV, yCamSpeed * Time.deltaTime);
        }
    }

    IEnumerator Dash(float time)
    {
        //dash here
        hasDashed = true;

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        if (isMoving)
        {
            dashDirection = transform.right * x + transform.forward * y;
        }
        else
        {
            dashDirection = transform.forward;

        }
        yield return new WaitForSeconds(time);
        hasDashed = false;
    }
}
