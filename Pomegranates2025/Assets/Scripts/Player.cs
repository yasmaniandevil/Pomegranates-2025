using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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

    public GameObject interactPopUp;
    //public Transform phoneSocket;
    public GameObject phonePrefab;
    public bool canPickUp = false;

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


        //after you pick it up just to turn it on and off from your hand
        //if you already picked it up
        if (phonePrefab.gameObject.activeInHierarchy)
        {
            //and you press E
            if (Input.GetKeyDown(KeyCode.E))
            {
                //you can turn it off and put it away
                phonePrefab.SetActive(false);
                canPickUp = !canPickUp;
            }
        }

        if (!canPickUp)
        {
            phonePrefab.SetActive(true);
            Debug.Log("take out of pocker");
        }

        /*if (canPickUp)
        {
            phonePrefab.SetActive(true);
            canPickUp=false;
        }*/

        /*if (Input.GetKeyDown(KeyCode.E))
        {
            canPickUp = !canPickUp;
        }*/

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

    //picking up the phone
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Phone"))
        {
            Debug.Log("in phone area");
            interactPopUp.SetActive(true );
            canPickUp = true;
            Debug.Log("can pick up bool: " + canPickUp);
            
            if (canPickUp && Input.GetKey(KeyCode.E))
            {
                Debug.Log("can pick ip is true and pressed E");
                phonePrefab.SetActive(true);
                canPickUp = false;
                //Debug.Log("Can pick up: " + canPickUp);
                other.gameObject.SetActive(false);
                
            }
            
        }
    }

    

    private void OnTriggerExit(Collider other)
    {

    }
}
