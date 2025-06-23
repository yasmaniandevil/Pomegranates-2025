using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class footSteps : MonoBehaviour
{
    public GameObject leftFoot;
    public GameObject rightFoot;

    public float spacing = .2f;

    bool isRightFootNext = false;

    public Player player;
    // Start is called before the first frame update
    void Start()
    {
        PlayFeet();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    void PlayFeet()
    {
        for(var i = 0; i < 10; i++)
        {
            //first instniatye the left one
            //if left one down instanite right one
            //go back and forth
            Vector3 distance = transform.position + transform.forward * spacing;
            Instantiate(leftFoot, distance, Quaternion.Euler(90, 0, 0));
            isRightFootNext = !isRightFootNext;
            if (isRightFootNext)
            {
                distance = distance * 2f;
                Instantiate(rightFoot, distance, Quaternion.Euler(90, 0, 0));
            }
            
            
        }

        
    }
}
