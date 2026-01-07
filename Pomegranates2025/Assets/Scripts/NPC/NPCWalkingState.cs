
using UnityEngine;
using UnityEngine.AI;

public class NPCWalkingState : NPCBaseState
{
    private Vector3 worldTargetPosition;
    private bool waklingToDest;
    public override void EnterState(NPCStateManager npc)
    {
        // Destination not set yet
        waklingToDest = false;

        // Get Destination
        //? Convert local to world => worldTargetPosition = transform.parent.TransformPoint(localTargetPosition) => Pass this into navMeshAgent
        //! Navmesh agent only operates in world space
        Vector3 localTargetPosition = npc.destinationList[npc.index];

        // Transforms position from local space to world space.
        if (npc.transform.parent != null)
        {
            worldTargetPosition = npc.transform.parent.TransformPoint(localTargetPosition);
        }
        else
        {
            worldTargetPosition = localTargetPosition;
        }

        
    }

    public override void UpdateState(NPCStateManager npc)
    {
        // Destination ideally only set once!
        if (!waklingToDest)
        {
           
            NavMeshHit hit;
            if (NavMesh.SamplePosition(worldTargetPosition, out hit, 15.0f, NavMesh.AllAreas))
            {
                // set animation and walking animation
                npc.SetDestination(hit.position);
                npc.SetAnimatorWalking(true);
                waklingToDest = true;
            }
            else
            {
                Debug.LogWarning("Could not find valid NavMesh point for destination: " + worldTargetPosition);
            }
        }

       
        // See if NavMeshAgent has reached destination
        Debug.Log("remaining distance: " + npc.GetAgent().remainingDistance);
        NavMeshAgent agent = npc.GetAgent();

        // See if NavMeshAgent has reached destination
        if (!agent.pathPending &&
            agent.hasPath &&
            agent.remainingDistance <= agent.stoppingDistance)
        {
            // Ensure npc is back to idle animation and then switch back to idle!
            npc.SetAnimatorWalking(false);

            // Make index increment so next time we enter walking state it's a new dest.
            npc.index++;
            if (npc.index >= npc.destinationList.Count)
                npc.index = 0;

            // Use switch state here
            // On walking state re-enter, waklingToDest bool is back to false + new destination chosen
            npc.SwitchState(npc.idleState);
        }


    }
}
