using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Hospital Cameras")]
    public Camera[] listOfCameras;
    public Shader securityCameraShader;

    private int camIndex = 0;
    private Camera currentCam;

    void Awake()
    {
        currentCam = listOfCameras[0];

        currentCam.gameObject.SetActive(true);
        currentCam.gameObject.tag = "MainCamera";
        camIndex = 0;

        for (int i = 0; i < listOfCameras.Length; i++)
        {
            listOfCameras[i].RenderWithShader(securityCameraShader, "");
        }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Input for Camera Index
        if (Input.GetKeyDown("e"))
        {
            camIndex += 1;

            if (camIndex >= listOfCameras.Length)
            {
                camIndex = listOfCameras.Length - 1;
            }
        }

        if (Input.GetKeyDown("q"))
        {
            camIndex -= 1;

            if (camIndex < 0)
            {
                camIndex = 0;
            }
        }

        // Camera switch 
        // If this statement is true then we are at the end or beginning of the round robin -- don't switch.
        if (listOfCameras[camIndex].gameObject.activeSelf == false)
        {
            currentCam.gameObject.SetActive(false);
            currentCam = listOfCameras[camIndex];
            currentCam.gameObject.SetActive(true);
        }


    }
}
