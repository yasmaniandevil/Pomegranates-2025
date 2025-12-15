using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour
{
    public Transform mainCam;
    public Vector3 offset;

    private Transform transformChild;


    void Awake()
    {
        transformChild = gameObject.transform.GetChild(0);
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transformChild.rotation = Quaternion.LookRotation(transformChild.position - mainCam.transform.position); // Always look at current Camera
        transformChild.position = transform.position + offset;
    }
}
