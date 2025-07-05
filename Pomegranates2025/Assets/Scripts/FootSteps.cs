using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;

public class footSteps : MonoBehaviour
{
    public GameObject leftFoot;
    public GameObject rightFoot;

    public float spacing = 1f;

    public bool isRightFootNext;

    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        
        //player = GetComponent<Player>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayFeet();


    }

    void PlayFeet()
    {
        Vector3 distance = transform.position + transform.forward * spacing;

        //if player is on left foot layer
        if (player.isOnLeftFootPrint)
        {
            //then right foot next is true
            isRightFootNext = true;
            //checks if true
            if (isRightFootNext)
            {
                //adds to the distance
                distance = distance * 2f;
                //instantiate right foot image
                Instantiate(rightFoot, distance, Quaternion.identity);
            }

        }//if player is on right footprint layer
        else if (player.isOnRightFootPrint)
        {
            //then instantiate left foot print
            isRightFootNext= false;
            if ((!isRightFootNext))
            {
                Instantiate(leftFoot, distance, Quaternion.identity);
            }
        }
            /*for (var i = 0; i < 10; i++)
            {
                //first instniatye the left one
                //if left one down instanite right one
                //go back and forth

                //Instantiate(leftFoot, distance, Quaternion.identity);
                isRightFootNext = !isRightFootNext;





            }*/ 

        

        
    }

    
    
}
