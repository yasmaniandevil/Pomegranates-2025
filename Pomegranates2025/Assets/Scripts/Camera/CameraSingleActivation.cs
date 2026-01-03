using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingleActivation : MonoBehaviour
{
    public GameObject[] gameObjectsToActivate;
    public bool basicCooldown = true;
    public bool finalCamera = false;
    public bool muteMusic;


    public CameraManager camManager;
    public AudioManager audioManager;



    public void ActivateAllGameObjects()
    {
        for (int i = 0; i < gameObjectsToActivate.Length; i++)
        {
            gameObjectsToActivate[i].SetActive(true);
        }

        if (muteMusic)
        {
            audioManager.MuteBGMusic();
        }
        else
        {
            audioManager.UnMuteBGMusic();
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
