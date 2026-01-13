
using System.Collections.Generic;
using UnityEngine;

public class LittleBoyPlayerAdrenalineState : LittleBoyPlayerBaseState
{
    // Bucket Actions
    private bool actionStart;
    private bool actionComplete;
    private float secondsFill = 4.0f;
    private float timeElapsed = 0.0f;

    // Movement Params

    // TODO: Generally these should be serialized values for when we re-use this code for other players but not for rn
    private float fovFastInit = 80.0f;
    private float fovFastFinal = 70.0f;
    private float fovSlow = 60.0f;
    private float moveSpeedSlow = 2.5f;
    private float moveSpeedFastInit = 7.0f;
    private float moveSpeedFastFinal = 5.5f;
    private float currSpeed;
    private float currentFOV;
    private Camera bucketCamera;
    private Camera playerCamera;

    private LittleBoyPlayerBody playerBody;

    public override void EnterState(LittleBoyPlayerStateManager player)
    {
        playerBody = player.GetPlayerBody();
        actionStart = false;
        actionComplete = false;
        playerBody.Lock(false);

        List<Camera> cameraList = playerBody.Cameras();
        playerCamera = cameraList[0];
        bucketCamera = cameraList[1];

        currentFOV = fovFastInit;
        currSpeed = moveSpeedFastInit;

    }

    public override void UpdateState(LittleBoyPlayerStateManager player)
    {
        // throw new System.NotImplementedException();
        // Interact handle is here now 

        movementTrack(player);
        bucketAction(player);
    }

    private void movementTrack(LittleBoyPlayerStateManager player)
    {


        // bucket is full
        // Player enters with adrenaline and the adrenaline leaves his body as he moves
        if (player.GetBucketState() == false)
        {
            if (!actionStart)
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
                    currSpeed = Mathf.MoveTowards(currSpeed, moveSpeedSlow, Time.deltaTime * 1.5f);

                    // Camera Adjust
                    playerCamera.fieldOfView = currentFOV;
                    bucketCamera.fieldOfView = currentFOV;

                    // Speed adjust
                    playerBody.moveSpeed = currSpeed;
                }
            }
        }
        else // In this case, the water bucket is empty, player gets energy back is and able to move fast!
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
                currentFOV = Mathf.MoveTowards(currentFOV, fovFastFinal, Time.deltaTime * 3.5f);
                currSpeed = Mathf.MoveTowards(currSpeed, moveSpeedFastFinal, Time.deltaTime * 1.5f);

                // Camera Adjust
                playerCamera.fieldOfView = currentFOV;
                bucketCamera.fieldOfView = currentFOV;

                // Speed adjust
                playerBody.moveSpeed = currSpeed;
            }
        }


    }

    private void bucketAction(LittleBoyPlayerStateManager player)
    {
        if (!actionComplete)
        {
            if ((player.GetRayHit() || actionStart) && playerBody.GetPlayerCharacterController().isGrounded)
            {
                // Button Pressed
                if (playerBody.GetPlayerInputHandler().InteractStarted)
                {
                    // Debug.Log("Interact pressed");

                    if (actionStart == false)
                    {
                        actionStart = true;

                        player.fillCanvas.SetActive(true);
                        player.progressBar.fillAmount = 0;

                        playerBody.Lock(true);
                        player.GetBucketAnimator().SetBool("Drain", true);
                    }

                    if (timeElapsed <= secondsFill)
                    {
                        timeElapsed += Time.deltaTime;
                        float targetFill = Mathf.Clamp01(timeElapsed / secondsFill);

                        player.progressBar.fillAmount = Mathf.MoveTowards(
                            player.progressBar.fillAmount,
                            targetFill,
                            Time.deltaTime * 1f // speed of visual fill, tweak as desired
                        );
                    }
                }

                if (playerBody.GetPlayerInputHandler().InteractCompleted)
                {
                    //StartInteraction();
                }

                if (playerBody.GetPlayerInputHandler().InteractCancelled)
                {
                    if (playerBody.GetPlayerInputHandler().WasInterruptedBeforeCompletion())
                    {
                        player.GetBucketAnimator().SetBool("Drain", false);

                        // CancelInteraction();

                    }
                    else
                    {
                        actionComplete = true;
                        player.SetBucketState(true);
                        player.GetBucketWater().SetActive(false);

                    }

                    playerBody.GetPlayerInputHandler().ResetInteractFlags();
                    actionStart = false;
                    playerBody.Lock(false);

                    player.fillCanvas.SetActive(false);
                    player.progressBar.fillAmount = 0;
                    timeElapsed = 0.0f;
                }
            }
        }

    }


}

// TODO: LOOK INTO CYCLE PARAMETER, NOT FOR RIGHT NOW
//? Maybe not we were able to figure this part out
// if setWaterPumpingInteract == true && filledWater == false

// have a float that over fillAmount = time.deltatime * waterfillfrequency fills up from 0 -> 100
// during water fill have animation for bucket fill progressively. Fill amount animation = fillAmount (speed of animation is tied to fillAmount)
// When water at max, stop water fill animation and set filledWater (new bool for player) to true    
