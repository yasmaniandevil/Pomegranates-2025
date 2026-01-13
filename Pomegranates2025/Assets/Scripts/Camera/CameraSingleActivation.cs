using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingleActivation : MonoBehaviour
{
    public GameObject[] gameObjectsToActivate;
    public bool basicCooldown = true;
    public bool finalCamera = false;
    public bool muteToggle;
    public bool muteMusic;
    public float fadeOutTime;
    public float fadeInTime;


    public CameraManager camManager;
    public AudioManager audioManager;


    public bool singleActivation;


    public void ActivateAllGameObjects()
    {
        singleActivation = true;
        for (int i = 0; i < gameObjectsToActivate.Length; i++)
        {
            gameObjectsToActivate[i].SetActive(true);
        }

        if (muteToggle)
        {
            if (muteMusic)
            {
                audioManager.MuteBGMusic(fadeOutTime);
            }
            else
            {
                audioManager.UnMuteBGMusic(fadeInTime);
            }
        }

    }

    public void DeActivateAllGameObjects()
    {
        for (int i = 0; i < gameObjectsToActivate.Length; i++)
        {
            gameObjectsToActivate[i].SetActive(false);
        }
    }

    public void EnableNext()
    {
        camManager.ShowNext();
    }

}
