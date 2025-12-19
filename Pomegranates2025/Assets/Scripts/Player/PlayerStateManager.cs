using UnityEngine;


public class PlayerStateManager : MonoBehaviour
{
    // Base State + Chil State Reference
    // For Player we have: IDLE + MOVEMENT + JUMPING + ACTION-TALKING + ACTION-WATER
    PlayerBaseState currentState;

    // Change this to bucket and no bucket

    /*
    public PlayerInsideState insideState = new PlayerInsideState();
    public PlayerOutsideState outsideState = new PlayerOutsideState();
    */

    // TODO: Replace the above states with new ones! We should be looking at the following
    // 1) Player Enter: The little boy, with his full bucket of water, starts off with a lot of adrenaline. FOV and decaying move speed are present
    // 2) Player Dread: The little boy, now with a present vinette we can use the unity URP post process volume, is a lot slower. Dread creeps in.
    // 3) Player Exit: The little boy dumps his water with a long interaction -- afterwards he is fast and light again but again this is decaying and we go back to 2) as he approaches the entrance



    // TODO: Replace the below trigger to account for above!
    // Location
    public enum Location
    {
        Inside,
        Outside
    }

    private Location currentLocation;

    void Start()
    {
        // State Init
        /*
        currentLocation = Location.Inside;
        currentState = insideState;
        insideState.EnterState(this);
        */

    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    // Switch States
    public void SwitchState(PlayerBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    // BOB LOGIC


    public Location GetLocation()
    {
        return currentLocation;
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