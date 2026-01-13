using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetupTrigger : MonoBehaviour
{
    public GameObject entryDoor;
    public GameObject exitDoor;
    public GameObject levelIncrementTrigger;
    public GameObject waterDropoffTrigger;

    // the exit door locks
    // the entry door locks
    // disable self
    // set level increment triggger to true
    void OnTriggerEnter(Collider other)
    {
        exitDoor.SetActive(true);
        entryDoor.SetActive(true);
        gameObject.SetActive(false);
        levelIncrementTrigger.SetActive(true);
        waterDropoffTrigger.SetActive(true);

    }
}
