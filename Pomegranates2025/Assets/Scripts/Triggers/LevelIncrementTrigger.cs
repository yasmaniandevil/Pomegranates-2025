using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class LevelIncrementTrigger : MonoBehaviour
{
    public int levelIncrement = 0;
    public GameObject entryDoor;
    public GameObject exitDoor;
    public GameObject resetTrigger;


    // the increment should increase,
    // the exit door is locked
    // the entry door is unlocked
    // disable self
    // set entry trigger to true
    void OnTriggerEnter(Collider other)
    {
        exitDoor.SetActive(true);
        entryDoor.SetActive(false);
        levelIncrement += 1;
        gameObject.SetActive(false);
        resetTrigger.SetActive(true);
    }
}
