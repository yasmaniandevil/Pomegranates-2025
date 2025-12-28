using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public Transform mainCam;
    public Vector3 offset;
    public Vector3 offsetRotation;

    private Transform transformChild;
    private Quaternion baseRotation;

    void Awake()
    {
        transformChild = gameObject.transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {
        // START with camera look and then as player moves it won't matter! This shouldn't be in update!
        baseRotation = Quaternion.LookRotation(transformChild.position - mainCam.transform.position); // Always look at current Camera

    }

    // Update is called once per frame
    void Update()
    {
        // move to start?
        transformChild.position = transform.position + offset;
        transformChild.rotation = baseRotation * Quaternion.Euler(offsetRotation);
    }
}
