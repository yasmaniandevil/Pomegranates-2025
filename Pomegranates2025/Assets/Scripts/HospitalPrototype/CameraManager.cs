using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Hospital Cameras - Render Texture Materials")]
    public Material[] listOfCameraRenderTextureMats;

    private int camIndex = 0;
    private MeshRenderer screenRenderer;
    private PlayerInput playerInput;

    void Awake()
    {
        screenRenderer = GetComponent<MeshRenderer>();
        playerInput = GetComponent<PlayerInput>();
    }


    // Start is called before the first frame update
    void Start()
    {
        screenRenderer.material = listOfCameraRenderTextureMats[camIndex];
    }

    // Update is called once per frame
    void Update()
    {


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

    public void ActivateActionMap()
    {
        playerInput.enabled = true;
    }
}
