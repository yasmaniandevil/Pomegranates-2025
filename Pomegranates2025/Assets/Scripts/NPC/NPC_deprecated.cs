using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class NPC : MonoBehaviour
{
    public bool walkable = false;
    public List<Vector3> destinationList;
    private Animator animator;
    private NavMeshAgent agent;

    private float timeRemaining;
    private int index;


    // First thing that is called -- used for init
    void Awake()
    {
        // Retrieve the animator + agent
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        agent.isStopped = false;
        // Reconsider the following, maybe just manually log first position for right now
        /*
        Vector2 currLocationConversion = new Vector2(currentLocation.x, currentLocation.y);
        destinationList.Add(currLocationConversion);
        */

        // Set timer + index
        timeRemaining = Random.Range(13.0f, 15.0f);
        index = 1;

        NavMeshHit hit;
        if (!NavMesh.SamplePosition(transform.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            Debug.LogError("NPC is not on the NavMesh!");
        }
    }

    // Start is called before the first frame update -- used for object communication
    void Start()
    {
        // Ensure that we are not walking! Base case error handling
        if (animator.GetBool("Walking") == true)
        {
            animator.SetBool("Walking", false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (walkable && destinationList.Count > 1)
        {
            // Do not tick down timer if we are moving
            if (timeRemaining > 0 && agent.velocity.magnitude == 0f)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Destination Set");
                Debug.Log(agent.enabled);
                // Reset Timer
                timeRemaining = Random.Range(13.0f, 15.0f);

                // Move NPC
                Vector3 rawDestination = destinationList[index];

                // TODO: IS NAVMESH GLOBAL OR LOCAL POSITION + CONSIDER PATH PENDING!!
                // ! NAVMESH ONLY OPERATES IN WORLD SPACE AAAAAAA

                //? Convert local to world => worldTargetPosition = transform.parent.TransformPoint(localTargetPosition) => Pass this into navMeshAgent
                /*
                NavMeshHit hit;
                if (NavMesh.SamplePosition(rawDestination, out hit, 10.0f, NavMesh.AllAreas))
                {
                    Debug.DrawLine(transform.position, hit.position, Color.green, 2.0f);
                    agent.SetDestination(hit.position);
                    Debug.Log("Setting destination to: " + hit.position);
                }
                else
                {
                    Debug.LogWarning("Could not find valid NavMesh point for destination: " + rawDestination);
                    Debug.DrawRay(rawDestination, Vector3.up * 2f, Color.red, 2.0f);
                }
                */
                agent.SetDestination(rawDestination);

                // Increment Index
                index += 1;

                if (index == destinationList.Count)
                {
                    index = 0;
                }
            }
        }

        // Control animation if speed detected!
        if (agent.velocity.magnitude != 0f)
        {
            Debug.Log("Animating Here");
            animator.SetBool("Walking", true);
        }
        else
        {
            animator.SetBool("Walking", false);
        }
    }

}
