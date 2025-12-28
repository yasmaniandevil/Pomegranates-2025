using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSingleActivation : MonoBehaviour
{
    public GameObject[] gameObjectsToActivate;
    public bool basicCooldown = true;
    public bool finalCamera = false;
    public CameraManager camManager;


    public void ActivateAllGameObjects()
    {
        for (int i = 0; i < gameObjectsToActivate.Length; i++)
        {
            gameObjectsToActivate[i].SetActive(true);
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
