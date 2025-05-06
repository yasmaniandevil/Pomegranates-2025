using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using VideoKit;

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

    public GameObject reticle;
    public GameObject interactPopUp;
    //public Transform phoneSocket;
    public GameObject phonePrefab;
    bool pickedUp;
    bool phoneInPocket = true;

    


    // Start is called before the first frame update
    void Start()
    {
        
        cc = GetComponent<CharacterController>();


        //playerCamera = GetComponentInChildren<Transform>();
        playerCamera = GameObject.FindWithTag("MainCamera");
        
        Debug.Log(playerCamera.name);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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

        PhoneInPocket();

        //reticle is always red
        reticle.GetComponent<Image>().color = Color.red;
       

        //shoot ray for reticle
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        //Shoots ray at 5 meters
        if (Physics.Raycast(ray, out hit, 5))
        {
            //if object has interactable tag
            if(hit.collider.CompareTag("Interactable"))
            {
                //turn reticle black
                reticle.GetComponent<Image>().color = Color.black;
            }
       
        }

        

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
        //enter trigger E to interact pop up UI
        if (other.gameObject.CompareTag("Interactable"))
        {
            interactPopUp.SetActive(true);
            
            //if we press E we grab the phone and destroy the pick up item
            //set picked up to true
            if (Input.GetKey(KeyCode.E))
            {
                
                phonePrefab.SetActive(true);
                //Debug.Log("Can pick up: " + canPickUp);
                other.gameObject.SetActive(false);
                pickedUp = true;
                
            }
            
        }
    }

    void PhoneInPocket()
    {
        
        //if we already picked it up
        if (pickedUp)
        {
            //we can press E
            if (Input.GetKeyDown(KeyCode.E))
            {
                //toggles it to true or false each time you picked it up
                phoneInPocket = !phoneInPocket;
                

                //if ur phone is not in ur pocket than u r using it
                if (!phoneInPocket)
                {
                    phonePrefab.SetActive(true);
                }
                //if true that your phone is in ur pocket it is no longer visible
                else
                {
                    phonePrefab.SetActive(false);
                }

            }
        }
    }

 
        
}
