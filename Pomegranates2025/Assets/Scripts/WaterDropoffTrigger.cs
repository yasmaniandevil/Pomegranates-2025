using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterDropoffTrigger : MonoBehaviour
{
    public GameObject exitDoor;

    // open exit door and disable self
    void OnTriggerEnter(Collider other)
    {
        exitDoor.SetActive(false);
        gameObject.SetActive(false);
    }
}
