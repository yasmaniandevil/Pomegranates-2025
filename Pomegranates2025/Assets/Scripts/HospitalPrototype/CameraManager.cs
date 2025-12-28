using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [Header("Hospital Cameras - Render Texture Materials")]
    public Material[] listOfCameraRenderTextureMats;

    [Header("Hospital Cameras - Camera Activation Scripts")]
    public CameraSingleActivation[] cameraActivations;

    [Header("Cutscene Manager")]
    public CutsceneManager cutsceneManager;

    public TMP_Text camText;
    public GameObject nextCamText;

    private int camIndex = 0;
    private int currMarker;
    private MeshRenderer screenRenderer;
    private PlayerInput playerInput;


    private bool cooldown = false;
    private WaitForSeconds cooldownDelay; // in seconds
    private Coroutine cooldownCoroutine;

    void Awake()
    {
        screenRenderer = GetComponent<MeshRenderer>();
        playerInput = GetComponent<PlayerInput>();

        currMarker = camIndex + 1;
        cooldownDelay = new WaitForSeconds(3.0f);
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


        // Activation
        cameraActivations[camIndex].ActivateAllGameObjects();
    }

    // Input for Camera Index

    void OnNext()
    {
        if (cooldown)
        {
            return;
        }

        camIndex += 1;

        if (camIndex >= listOfCameraRenderTextureMats.Length)
        {
            camIndex = listOfCameraRenderTextureMats.Length - 1;

            if (cameraActivations[camIndex].finalCamera)
            {
                // We need to start the final cutscene!!!!!
                cutsceneManager.PlayNext();
            }
        }
        else
        {
            // Need to deactivate previous camInd game objects
            // This will never hit index 0
            cameraActivations[camIndex - 1].DeActivateAllGameObjects();
            AudioManager.instance.PlayGlobalSFX("CamClick");

        }

        cooldown = true;

        HideNext();

        // The following is a time based cooldown
        // If Camera Activation has below bool set to true, camera manager is responsible for re-enable of Next
        // Otherwise it will be tied to outside gameobject that will shoot a signal when it is done!

        if (cameraActivations[camIndex].basicCooldown)
        {

            if (cooldownCoroutine != null)
            {
                StopCoroutine(cooldownCoroutine);
            }
            // Debug.Log("Basic Start");
            cooldownCoroutine = StartCoroutine(Cooldown());
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

    private IEnumerator Cooldown()
    {
        // Debug.Log("Cooldowning...");
        yield return cooldownDelay;
        // Debug.Log("Cooldown Complete");
        ShowNext();
    }

    public void ActivateActionMap()
    {
        playerInput.enabled = true;
    }

    public void ShowNext()
    {
        nextCamText.gameObject.SetActive(true);
        cooldown = false; // this will help us in the long run
    }

    public void HideNext()
    {
        nextCamText.gameObject.SetActive(false);
    }


}
