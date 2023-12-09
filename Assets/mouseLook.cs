using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mouseLook : MonoBehaviour
{
    public float mouseSensitivityX;
    public float mouseSensitivityY;

    public Transform playerBody;

    float xRotation;
    float yRotation;
    float zRotation;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    } 

    void Update()
    {
        LookUpdate();
    }

    void LookUpdate()
    {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        float xMove = Input.GetAxis("Horizontal");
        if (xMove > 0.1f)
        {
            zRotation = Mathf.Lerp(zRotation, -1, 6 * Time.deltaTime);

        }
        else if (xMove < -0.1f)
        {
            zRotation = Mathf.Lerp(zRotation, 1, 6 * Time.deltaTime);
        }
        else
        {
            zRotation = Mathf.Lerp(zRotation, 0, 6 * Time.deltaTime);
        }

        float yMove = Input.GetAxis("Vertical");
        if (yMove > 0.1f)
        {
            yRotation = Mathf.Lerp(yRotation, 2, 12 * Time.deltaTime);
        }
        else if (yMove < -0.1f)
        {
            yRotation = Mathf.Lerp(yRotation, -2, 12 * Time.deltaTime);
        }
        else
        {
            yRotation = Mathf.Lerp(yRotation, 0, 6 * Time.deltaTime);
        }


        transform.localRotation = Quaternion.Euler(xRotation, 0f, zRotation);
        transform.parent.transform.localRotation = Quaternion.Euler(yRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
