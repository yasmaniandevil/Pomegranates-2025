using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
   public List<Camera> cameras;

    // Start is called before the first frame update
    void Start()
    {

        SwitchCamera(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //for any of the cameras in our list
        //loops thru them
        //if we have 4 camera i goes from 0 to 3
        for (int i = 0; i < cameras.Count; i++)
        {
            //maps index 0 to alpha 0 key and so on
            //checks if any number key matching an index is pressed
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                SwitchCamera(i);
            }
        }

    }

    //function to switch cam at the index
    void SwitchCamera(int camNumber)
    {
        //loops over our cameras
        for(int i = 0; i< cameras.Count;i++)
        {
            //i == camNumber checks if this is the camera we want to turn on (true/false)
            if(i == camNumber)
            {
                cameras[i].gameObject.SetActive(true);
            }
            else
            {
                cameras[i].gameObject.SetActive(false);
            }
        }
        
    }
}
