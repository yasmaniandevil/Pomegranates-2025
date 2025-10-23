using UnityEngine;
using UnityEngine.AI;

public class PlayerOutsideState : PlayerBaseState
{
    private float camSpeed = 10.0f;
    private float destFOV = 75.0f;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("I'm outisde");
    }

    public override void UpdateState(PlayerStateManager player)
    {
        //? WHILE LOOPS RUN IN A SINGLE FRAME AND NOT OVER TIME EVEN WITH TIME.DELTATIME!
        // When entering outside from inside we should be speeding up
        if (player.moveSpeed < 6.0f)
        {
            player.moveSpeed += 0.7f * Time.deltaTime;
            player.moveSpeed = Mathf.Min(player.moveSpeed, 6.0f); // clamp
        }

        // Adjust FOV gradually -- visible from outside to inside
        player.playerCamera.fieldOfView = Mathf.MoveTowards(player.playerCamera.fieldOfView, destFOV, Time.deltaTime * camSpeed);


        // Location Check
        PlayerStateManager.Location currentLocation = player.GetLocation();

        if (currentLocation == PlayerStateManager.Location.Inside)
        {
            player.SwitchState(player.insideState);
        }
        return;
    }
}