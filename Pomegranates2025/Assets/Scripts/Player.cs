using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    CharacterController cc;
    float moveSpeed = 5f;

    //camera look varrs
    //how fast camera moves
    public float mouseSensitivity;
    GameObject playerCamera;
    //track camera vertical- up and down movement
    private float yRotation = 0;
    private float xRotation = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        cc = GetComponent<CharacterController>();

        //playerCamera = GetComponentInChildren<Transform>();
        playerCamera = GameObject.FindWithTag("MainCamera");
        
        Debug.Log(playerCamera.name);

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalX = Input.GetAxis("Horizontal");
        float forwardZ = Input.GetAxis("Vertical");
        Vector3 move;

        move = (transform.right * horizontalX) + (transform.forward * forwardZ);

        cc.Move(move * moveSpeed * Time.deltaTime);

        CameraLook();

    }

    void CameraLook()
    {
        //get and assign mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        //rotation around the y axis (look left and right)
        //when the mouse moves horizontally (x)
        yRotation += mouseX;
        //rotate the player left/rigght y axis rotation
        transform.rotation = Quaternion.Euler(0f, yRotation, 0);

        //Decrease xRotation when moving mouse up so camera tilts up
        //increaste x rotation when moving camera down so camera tilts down
        //rotate around the x axis( up and down)
        xRotation -= mouseY;
        //clamp rotation
        xRotation = Mathf.Clamp(xRotation, -90, 90); //prevent flipping

        
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
    }

    private void OnMouseDown()
    {
        if (gameObject.CompareTag("Phone"))
        {
            Debug.Log("pickup ohne");
        }
    }
}
