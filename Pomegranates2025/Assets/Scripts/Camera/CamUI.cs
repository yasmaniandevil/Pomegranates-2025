using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamUI : MonoBehaviour
{

    public GameObject CamUIGameObject;

    public void toggleActive()
    {
        //Debug.Log("Meow");
        CamUIGameObject.SetActive(!CamUIGameObject.activeSelf);
        //CamUIGameObject.gameObject.SetActive(!!CamUIGameObject.gameObject.activeSelf);
    }
}
