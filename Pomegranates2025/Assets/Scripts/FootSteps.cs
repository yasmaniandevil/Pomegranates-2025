using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class footSteps : MonoBehaviour
{
    float distance;

    public float spacing = 10f;

    public bool isRightFootNext;

    public Transform lastFootPrint;

    public Player player;

    public GameObject leftFoot;
    public GameObject rightFoot;

    public List<GameObject> feets = new List<GameObject>();

    Vector3 currentDir;
    public Paths pathData;
    int currentDirectionIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        
        currentDir = pathData.startingPos.forward;
    }

    // Update is called once per frame
    void Update()
    {
        PlayFeet();

        //make sure the player isnt too far away
        distance = Vector3.Distance(lastFootPrint.position, player.transform.position);
        Debug.Log("distance: " + distance);
    }

    void PlayFeet()
    {
        if (feets.Count < 15)
        {
            //further or closer to the footsteps for them to spawn?
            if (distance <= pathData.spacing)
            {

                GameObject newFoot;
                Vector3 spawnPos = lastFootPrint.position + currentDir * pathData.spacing;

                if (isRightFootNext)
                {
                    newFoot = Instantiate(pathData.rightFoot, spawnPos, rightFoot.transform.rotation);
                    feets.Add(newFoot);
                }
                else
                {
                    newFoot = (Instantiate(pathData.leftFoot, spawnPos, leftFoot.transform.rotation));
                    feets.Add(newFoot);
                }

                lastFootPrint = newFoot.transform;
                isRightFootNext = !isRightFootNext;

                if(feets.Count == 7 && currentDirectionIndex < pathData.directionChanges.Count)
                {
                    float angle = pathData.directionChanges[currentDirectionIndex];
                    Quaternion turn = Quaternion.Euler(0, angle, 0);
                    currentDir = turn * currentDir;

                    currentDirectionIndex++; //move to next direction change for next time
                }


            }
        }

        

        

        
    }

    

    
    
}
