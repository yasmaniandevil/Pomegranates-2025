
using UnityEngine;
using UnityEngine.AI;

public class NPCIdleState : NPCBaseState
{

    private float timeRemaining;


    public override void EnterState(NPCStateManager npc)
    {
        // Reset timer
        timeRemaining = Random.Range(13.0f, 15.0f);

        // Ensure that we are not walking! Base case error handling
        Animator temp = npc.GetAnimator();

        if (temp.GetBool("Walking") == true)
        {
            temp.SetBool("Walking", false);
        }
    }

    public override void UpdateState(NPCStateManager npc)
    {
        // Check if we can even leave idle state in the first place
        if (npc.walkable && npc.destinationList.Count > 1)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                // Use switch state here
                // When we call this, we don't call this update state anymore
                // In addition, when we re-call idle state, timer is reset!
                npc.SwitchState(npc.walkingState);
            }
        }
    }
}
