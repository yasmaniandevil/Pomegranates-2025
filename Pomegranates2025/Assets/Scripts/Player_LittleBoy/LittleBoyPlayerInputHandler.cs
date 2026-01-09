using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LittleBoyPlayerInputHandler : MonoBehaviour
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
    // public bool InteractHeld { get; private set; }

    // Public state (read-only)
    public bool InteractStarted { get; private set; }
    public bool InteractCompleted { get; private set; }
    public bool InteractCancelled { get; private set; }



    public bool JumpTriggered { get; private set; }
    public Vector2 RotationInput { get; private set; }

    void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        interactAction = mapReference.FindAction(interact);
        jumpAction = mapReference.FindAction(jump);
        rotationAction = mapReference.FindAction(rotation);

        int bindingIndex = interactAction.bindings
        .IndexOf(b => b.path == "<Keyboard>/e");


        interactAction.ApplyBindingOverride(
            bindingIndex,
            new InputBinding
            {
                overrideInteractions = "hold(duration=4,pressPoint=0.5)"
            }
        );


        SubscribeActionValuesToInputEvents();
    }

    private void SubscribeActionValuesToInputEvents()
    {
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

        /*
        interactAction.started += inputInfo => InteractHeld = true; // Fires when the key goes down
        interactAction.performed += inputInfo => InteractTriggered = true; // fires after the hold time completes
        interactAction.canceled += inputInfo => InteractHeld = false; // fires when the key is released
        */

        interactAction.started += OnInteractStarted;
        interactAction.performed += OnInteractPerformed;
        interactAction.canceled += OnInteractCanceled;

    }

    private void OnInteractStarted(InputAction.CallbackContext ctx)
    {
        // New interaction cycle
        InteractStarted = true;
        InteractCompleted = false;
        InteractCancelled = false;
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        // Hold finished (after 4 seconds)
        InteractCompleted = true;
    }

    private void OnInteractCanceled(InputAction.CallbackContext ctx)
    {
        // Button released (before or after completion)
        InteractCancelled = true;
    }

    public bool WasInterruptedBeforeCompletion()
    {
        return InteractCancelled && !InteractCompleted;
    }

    public bool WasReleasedAfterCompletion()
    {
        return InteractCancelled && InteractCompleted;
    }

    // Call this once you've consumed the input
    public void ResetInteractFlags()
    {
        InteractStarted = false;
        InteractCompleted = false;
        InteractCancelled = false;
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