using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;
using System;

public class LittleBoyPlayerDreadState : LittleBoyPlayerBaseState
{
    private LittleBoyPlayerBody playerBody;
    private float currentFOV;
    private float currSpeed;
    private float moveSpeedSlow = 1.0f;

    private float moveSpeedFastInit = 7.0f;
    private float fovSlow = 60.0f;
    private float fovFastInit = 80.0f;
    private Camera bucketCamera;
    private Camera playerCamera;



    private GameObject dreadObject;
    private Volume ppv;
    private Vignette vignette;
    private MotionBlur motionBlur;
    private FilmGrain filmGrain;



    private float vignetteBase = 0.2f;
    private float vignetteHigh = 0.45f;
    private float currVignette;

    private float motionBlurBase = 0.2f;
    private float motionBlurHigh = 0.8f;
    private float currmotionBlur;

    private float filmGrainBase = 0.0f;
    private float filmGrainHigh = 1.0f;
    private float currFilmGrain;

    private float distance;

    public override void EnterState(LittleBoyPlayerStateManager player)
    {
        playerBody = player.GetPlayerBody();
        playerBody.Lock(false);
        dreadObject = player.GetDreadObject();
        ppv = player.GetPPV();


        List<Camera> cameraList = playerBody.Cameras();
        playerCamera = cameraList[0];
        bucketCamera = cameraList[1];

        currentFOV = fovFastInit;
        currSpeed = moveSpeedFastInit;

        currVignette = vignetteBase;
        currmotionBlur = motionBlurBase;
        currFilmGrain = filmGrainBase;

        // Get effects
        if (ppv.profile.TryGet(out vignette))
        {
            vignette.active = true;
        }
        if (ppv.profile.TryGet(out filmGrain))
        {
            filmGrain.active = true;
        }
        if (ppv.profile.TryGet(out motionBlur))
        {
            motionBlur.active = true;
        }

    }

    public override void UpdateState(LittleBoyPlayerStateManager player)
    {
        distance = Vector3.Distance(player.transform.position, dreadObject.transform.position);

        effects(player);

    }

    private void effects(LittleBoyPlayerStateManager player)
    {
        // as distance shrinks, effects grow
        // TODO: REVIEW LERP AND INVERSELERP -- I DON'T UNDERSTAND THIS ENOUGH
        float t = Mathf.InverseLerp(40f, 20f, distance);
        float tspeed = Mathf.InverseLerp(45f, 20f, distance);

        currVignette = Mathf.Lerp(vignetteBase, vignetteHigh, t);
        currmotionBlur = Mathf.Lerp(motionBlurBase, motionBlurHigh, t);
        currFilmGrain = Mathf.Lerp(filmGrainBase, filmGrainHigh, t);
        currSpeed = Mathf.Lerp(moveSpeedFastInit, moveSpeedSlow, tspeed);


        vignette.intensity.Override(currVignette);
        motionBlur.intensity.Override(currmotionBlur);
        filmGrain.intensity.Override(currFilmGrain);
        playerBody.moveSpeed = currSpeed;
    }


    private void movementTrack(LittleBoyPlayerStateManager player)
    {
        // If boy is standing still no need to do this!
        if (
            playerBody.GetPlayerInputHandler().MovementInput.x == 0.0f &&
            playerBody.GetPlayerInputHandler().MovementInput.y == 0.0
        )
        {
            // do nothing
        }
        else
        {
            currentFOV = Mathf.MoveTowards(currentFOV, fovSlow, Time.deltaTime * 3.5f);
            // currSpeed = Mathf.MoveTowards(currSpeed, moveSpeedSlow, Time.deltaTime * 1.5f);

            // Camera Adjust
            playerCamera.fieldOfView = currentFOV;
            bucketCamera.fieldOfView = currentFOV;

            // Speed adjust
            // playerBody.moveSpeed = currSpeed;
        }
    }
}
