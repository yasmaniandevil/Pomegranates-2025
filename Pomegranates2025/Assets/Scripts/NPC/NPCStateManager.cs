using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCStateManager : MonoBehaviour
{

    // Base State + Child State Reference
    NPCBaseState currentState;
    public NPCIdleState idleState = new NPCIdleState();
    public NPCWalkingState walkingState = new NPCWalkingState();

    // Methods
    public bool walkable = false;
    public List<Vector3> destinationList; // ! THESE ARE LOCAL SPACE, CONVERT TO WORLD SPACE BEFORE SET DEST
    private Animator animator;
    private NavMeshAgent agent;
    public int index;

    public float lowRangeTimer = 13.0f;
    public float highRangeTimer = 15.0f;

    // First thing called -- used for init
    void Awake()
    {
        // Retrieve the animator + agent
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        // Just in case
        agent.isStopped = false;

        // set index to +1
        index = 1;


        Vector3 worldSourcePosition;

        if (transform.parent != null)
        {
            worldSourcePosition = transform.parent.TransformPoint(transform.localPosition);
        }
        else
        {
            worldSourcePosition = transform.position;
        }

        NavMeshHit hit;
        // If you use the keyword “out” then the function must set a value to that variable, i.e. it must output a value.
        if (!NavMesh.SamplePosition(worldSourcePosition, out hit, 1.0f, NavMesh.AllAreas))
        {
            Debug.LogError("NPC is not on NavMesh!");
        }

    }

    // Start is called before the first frame update -- usdd for game obj communication
    void Start()
    {
        currentState = idleState; // All NPCs need to start idl
        idleState.EnterState(this);

        // For initialization reasons, need to do this part at Start
        destinationList.Insert(0, transform.localPosition);

    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    // Switch States
    public void SwitchState(NPCBaseState state)
    {
        currentState = state;
        state.EnterState(this);
   
    }

    public Animator GetAnimator()
    {
        return animator;
    }

    public NavMeshAgent GetAgent()
    {
        return agent;
    }

    public void SetAnimatorWalking(bool setWalking)
    {
        animator.SetBool("Walking", setWalking);
        Debug.Log("walking bool: " + setWalking);
    }

    public void SetDestination(Vector3 destination)
    {
        agent.SetDestination(destination);
    }
}
