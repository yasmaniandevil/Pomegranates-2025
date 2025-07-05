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

    void SwitchCamera(int camNumber)
    {
        for(int i = 0; i< cameras.Count;i++)
        {
            cameras[i].gameObject.SetActive(i == camNumber);
        }
        
    }
}
