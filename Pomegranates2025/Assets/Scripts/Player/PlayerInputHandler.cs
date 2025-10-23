using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string rotation = "Rotation";

    // Input Actions
    private InputAction movementAction;
    private InputAction interactAction;
    private InputAction jumpAction;
    private InputAction rotationAction;

    // Input action getters and setters
    public Vector2 MovementInput { get; private set; }
    public bool InteractTriggered { get; private set; }
    public bool JumpTriggered { get; private set; }
    public Vector2 RotationInput { get; private set; }

    void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        interactAction = mapReference.FindAction(interact);
        jumpAction = mapReference.FindAction(jump);
        rotationAction = mapReference.FindAction(rotation);

        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
        Debug.Log("Subscribed");
        // performed happens every time movement action changes
        // += to subscribe and inputInfo is our temporary context
        // => is a lambda declaration
        // the lambda function takes whatever is passed into the context and applies it to our setter
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();

        // cancelled happens when no movement action
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        // now for our bool actions
        jumpAction.performed += inputInfo => JumpTriggered = true;
        jumpAction.canceled += inputInfo => JumpTriggered = false;

        interactAction.started += inputInfo => InteractTriggered = true;
        interactAction.performed += inputInfo => InteractTriggered = true;
        interactAction.canceled += inputInfo => InteractTriggered = false;

    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }
}