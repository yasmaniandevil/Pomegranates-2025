using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class PlayerInsideState : PlayerBaseState
{
    private float camSpeed = 10.0f;
    private float destFOV = 60.0f;

    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm inside");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        /*
        //? WHILE LOOPS RUN IN A SINGLE FRAME AND NOT OVER TIME EVEN WITH TIME.DELTATIME!
        // When entering inside from outside we should be slowing down
        if (player.moveSpeed > 4.0f)
        {
            player.moveSpeed -= 1.0f * Time.deltaTime;
            player.moveSpeed = Mathf.Max(player.moveSpeed, 4.0f); // clamp to avoid overshooting
        }

        // Adjust FOV gradually -- visible from outside to inside
        player.playerCamera.fieldOfView = Mathf.MoveTowards(player.playerCamera.fieldOfView, destFOV, Time.deltaTime * camSpeed);

        // Location Check
        PlayerStateManager.Location currentLocation = player.GetLocation();

        if (currentLocation == PlayerStateManager.Location.Outside)
        {
            player.SwitchState(player.outsideState);
        }
        return;
        */
    }
}