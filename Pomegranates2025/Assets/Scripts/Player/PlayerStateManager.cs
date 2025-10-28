using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class PlayerStateManager : MonoBehaviour
{
    [Header("Controls")]
    public float moveSpeed = 4.0f;
    public float gravityMultiplier = 2;
    public float jumpForce = 5.0f;
    public float mouseSensitivity = 1f;
    public float upDownLookRange = 90.0f; // Track camera vertical - up and down movement
    public bool enableBob = true; // head bob!

    [Header("Acceleration Settings")]
    public float accelerationTime = 0.1f;
    public float decelerationTime = 0.2f;

    [Header("Bucket Settings")]
    [SerializeField] private GameObject bucket;
    [SerializeField] private Animator bucketAnimator;
    [SerializeField] private Transform bucketTransform;

    private bool bucketShow = false;
    private bool handleInteract = false;
    private Vector3 bucketStartPos;
    private Vector3 bucketEndPos;
    private float smoothFactor = 7.0f;

    // Head bob 

    // these should increase with our set speed
    // Running: more frequency with maybe a little more amplitude (increases with speed)
    // Water: more bob, less frequency
    [Header("Bob Settings")]
    [SerializeField, Range(0, 0.1f)] private float bobAmplitude = 0.005f;
    [SerializeField, Range(0, 30)] private float bobFrequency = 10f;
    private Vector3 startPosBob;
    private float bobTimerHori = 0f;
    private float bobTimerVert = 0f;
    private float currentBobAmplitude;
    private float currentBobFrequency;

    // Base State + Chil State Reference
    // For Player we have: IDLE + MOVEMENT + JUMPING + ACTION-TALKING + ACTION-WATER
    PlayerBaseState currentState;

    public PlayerInsideState insideState = new PlayerInsideState();
    public PlayerOutsideState outsideState = new PlayerOutsideState();

    // Methods and Vars
    private Animator animator;
    private CharacterController cc;

    [SerializeField] private PlayerInputHandler playerInputHandler;
    [SerializeField] private Transform cameraHolder;


    // Current Movement
    private Vector3 currentMovement;
    private Vector3 currentVelocity = Vector3.zero;
    private float verticalRotation;
    private float currentSpeed => moveSpeed;

    // Curren rotation + smoothing
    private Vector2 currentRotation;
    private Vector2 smoothVelocity;
    private float smoothTime = 0.05f;

    // Location
    public enum Location
    {
        Inside,
        Outside
    }

    private Location currentLocation;

    // Camera
    public Camera playerCamera;

    // Reticle
    public GameObject reticle; // TODO: FIND RETICLE!!!

    // First thing called -- used for init
    void Awake()
    {
        // Retrieve animator
        animator = GetComponent<Animator>();

        // Retrieve character controller and player input
        cc = GetComponent<CharacterController>();

        // Bob Start Position
        startPosBob = playerCamera.transform.localPosition;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // State Init
        currentLocation = Location.Inside;
        currentState = insideState;
        insideState.EnterState(this);

        // Bucket settings
        handleInteract = false;
        bucketStartPos = bucketTransform.localPosition;
        bucketEndPos = new Vector3(bucketStartPos.x, bucketStartPos.y + 0.4f, bucketStartPos.z);
    }

    void Update()
    {
        HandleMovement();
        if (enableBob)
        {
            CheckMotion();
            // playerCamera.transform.LookAt(FocusTarget()); // for stabilizing and focusing on objects
        }
        HandleRotation();
        HandleInteract();
        // HandleMovement and HandleRotation
        // Handle input and logic update

        currentState.UpdateState(this);
    }

    // Switch States
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    // BOB LOGIC
    private void CheckMotion()
    {
        float speed = new Vector3(cc.velocity.x, 0, cc.velocity.z).magnitude;
        float speedFactor = Mathf.Clamp01(speed / moveSpeed); // increase bob when speed is increased -- only works to a top speed

        // Add bob motion based on speed
        float bobSpeed = Mathf.Clamp(speed * bobFrequency, 1f, 100f);

        bool isMoving = speed > 0.1f && cc.isGrounded;

        //Smoothly interpolate amplitude and frequency based on motion
        float targetAmp = isMoving ? bobAmplitude * speedFactor * bobSpeed : 0f;
        float targetFreq = isMoving ? bobFrequency * speedFactor * bobSpeed : 0f;

        // Amplitude needs to fade smoothly so when movement stops, bob amplitude lerps to 0 instead of snapping.
        currentBobAmplitude = Mathf.Lerp(currentBobAmplitude, targetAmp, 10f * Time.deltaTime);
        currentBobFrequency = Mathf.Lerp(currentBobFrequency, targetFreq, 10f * Time.deltaTime);

        // We only lerp back to start when amplitude is effectively 0
        if (currentBobAmplitude < 0.001f) // basically stopped
        {
            // Snap back to resting position
            playerCamera.transform.localPosition = Vector3.Lerp(playerCamera.transform.localPosition, startPosBob, 8f * Time.deltaTime);
        }

        // Advance timer based on movement speed -- bob offset is time-based so we can deal with sudden velocity changes
        bobTimerHori += Time.deltaTime * currentBobFrequency / 2;
        bobTimerVert += Time.deltaTime * currentBobFrequency;

        // Compute bob RELATIVE to start position
        Vector3 offset = new Vector3(
            Mathf.Cos(bobTimerHori) * currentBobAmplitude * 2, // horizontal sway
            Mathf.Sin(bobTimerVert) * currentBobAmplitude, // vertical bob
            0
        );

        playerCamera.transform.localPosition = startPosBob + offset; // no motion drift, camera position is ALWAYS computed as our startPos + offset so no cumulative error

    }

    /*
    private Vector3 FocusTarget()
    {
        // takes camera holder position, sets a point to 15 units in front of it
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y + transform.position.z);
        pos += cameraHolder.forward * 15.0f;
        return pos;
    }
    */

    // BOB LOGIC END

    public CharacterController GetCharacterController()
    {
        return cc;
    }

    public Location GetLocation()
    {
        return currentLocation;
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    private Vector3 CalculateWorldDirection()
    {
        Vector3 inputDirection = new Vector3(playerInputHandler.MovementInput.x, 0f, playerInputHandler.MovementInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);
        return worldDirection.normalized; // account for magnitude
    }

    // idk if we ever handle jumping, ask Yas
    private void HandleJumping()
    {
        if (cc.isGrounded)
        {
            currentMovement.y = -0.5f;
            if (playerInputHandler.JumpTriggered == true)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y += Physics.gravity.y * gravityMultiplier * Time.deltaTime;
        }
    }


    // LOCK THIS TO CERTAIN STATES!!!!! 
    private void HandleInteract()
    {
        // Reticle
        reticle.GetComponent<Image>().color = Color.red;

        //shoot ray for reticle
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;


        //Shoots ray at 5 meters
        if (Physics.Raycast(ray, out hit, 5))
        {
            //if object has interactable tag
            if (hit.collider.CompareTag("Interactable"))
            {
                //turn reticle black
                reticle.GetComponent<Image>().color = Color.black;

                if (hit.collider.name == "WaterPumpCollider")
                {
                    bucketShow = true;
                    if (playerInputHandler.InteractTriggered == true)
                    {
                        Debug.Log("Pressed");
                        handleInteract = true;
                    }

                    //Debug.Log("hit water collider");
                    /*
                    if (Input.GetMouseButtonDown(0))
                    {
                        AnimationPlay animation = GetComponent<AnimationPlay>();
                        Debug.Log("Got animation");
                        //animation.PlayAnimation();
                    }
                    */
                }
                // add a bool to handleInteract begin water pump
                // setWaterPumpingInteract = true
            }

        }
        else
        {
            bucketShow = false;
        }

        // Bucket values
        // bucket is at default position
        float distance = Vector3.Distance(bucketTransform.localPosition, bucketStartPos);
        bucket.SetActive(distance > 0.05f);

        Vector3 targetPos = bucketShow ? bucketEndPos : bucketStartPos;
        bucketTransform.localPosition = Vector3.Lerp(bucketTransform.localPosition, targetPos, Time.deltaTime * smoothFactor);

        bucketAnimator.SetBool("Filling", handleInteract);

        // TODO: LOOK INTO CYCLE PARAMETER, NOT FOR RIGHT NOW
        // if setWaterPumpingInteract == true && filledWater == false

        // have a float that over fillAmount = time.deltatime * waterfillfrequency fills up from 0 -> 100
        // during water fill have animation for bucket fill progressively. Fill amount animation = fillAmount (speed of animation is tied to fillAmount)
        // When water at max, stop water fill animation and set filledWater (new bool for player) to true    
    }

    // Movement Handling - CHECK BOOL OF STATE AND LOCK IT IF NECESSARY
    private void HandleMovement()
    {
        Vector3 worldDirection = CalculateWorldDirection();

        //Desired movement velocity
        Vector3 targetVelocity = worldDirection * currentSpeed;

        // Smooth acceleration and deceleration
        float smoothTime = (targetVelocity.magnitude > 0.1f) ? accelerationTime : decelerationTime;
        Vector3 horizontalVelocity = Vector3.SmoothDamp(
            new Vector3(currentMovement.x, 0, currentMovement.z),
            new Vector3(targetVelocity.x, 0, targetVelocity.z),
            ref currentVelocity,
            smoothTime
        );

        currentMovement.x = horizontalVelocity.x;
        currentMovement.z = horizontalVelocity.z;
        HandleJumping();
        cc.Move(currentMovement * Time.deltaTime);
    }

    // CameraLook() from deprecated - CHECK BOOL OF STATE AND LOCK IT IF NECESSARY
    private void HandleRotation()
    {
        // Debug.Log(playerInputHandler.RotationInput);
        Vector2 targetRotation = playerInputHandler.RotationInput * mouseSensitivity; // no mult by Time.deltaTime since we are already using delta movements for mouse
        currentRotation = Vector2.SmoothDamp(currentRotation, targetRotation, ref smoothVelocity, smoothTime);

        /*
        float mouseXRotation = playerInputHandler.RotationInput.x * mouseSensitivity; 
        float mouseYRotation = playerInputHandler.RotationInput.y * mouseSensitivity;
        */

        transform.Rotate(0, currentRotation.x, 0);

        // We only want to rotate the camera, not the whole player
        verticalRotation = Mathf.Clamp(verticalRotation - currentRotation.y, -upDownLookRange, upDownLookRange);
        cameraHolder.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // Detects collision
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string floorTag = hit.collider.tag;
        if (floorTag != "Inside" && floorTag != "Outside")
        {
            return;
        }


        if (floorTag == "Inside")
        {
            currentLocation = Location.Inside;
        }
        else if (floorTag == "Outside")
        {
            currentLocation = Location.Outside;
        }

    }
}