using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Hospital Cameras - Render Texture Materials")]
    public Material[] listOfCameraRenderTextureMats;
    public TMP_Text camText;

    private int camIndex = 0;
    private int currMarker;
    private MeshRenderer screenRenderer;
    private PlayerInput playerInput;

    void Awake()
    {
        screenRenderer = GetComponent<MeshRenderer>();
        playerInput = GetComponent<PlayerInput>();

        currMarker = camIndex + 1;
    }


    // Start is called before the first frame update
    void Start()
    {
        screenRenderer.material = listOfCameraRenderTextureMats[camIndex];

        camText.text = $"Cam {currMarker}";
    }

    // Update is called once per frame
    void Update()
    {
        if (currMarker != camIndex + 1)
        {
            // camIndex has updated
            currMarker = camIndex + 1;

            // Update TMP
            camText.text = $"Cam {currMarker}";
        }

        // Camera switch 
        // If this statement is true then we are at the end or beginning of the round robin -- don't switch.
        if (screenRenderer.material != listOfCameraRenderTextureMats[camIndex])
        {
            screenRenderer.material = listOfCameraRenderTextureMats[camIndex];
        }
    }

    // Input for Camera Index

    void OnNext()
    {
        camIndex += 1;

        if (camIndex >= listOfCameraRenderTextureMats.Length)
        {
            camIndex = listOfCameraRenderTextureMats.Length - 1;
        }
        else
        {
            AudioManager.instance.PlayGlobalSFX("CamClick");

        }
    }

    /*
    void OnPrev()
    {
        camIndex -= 1;
        if (camIndex < 0)
        {
            camIndex = 0;
        }
        else
        {
            AudioManager.instance.PlayGlobalSFX("CamClick");

        }
    }
    */

    public void ActivateActionMap()
    {
        playerInput.enabled = true;
    }
}
