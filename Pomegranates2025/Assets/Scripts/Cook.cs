using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    private Vector3 point;
    public bool dragging = false;
    private float distance;

    Rigidbody foodBody;

    // Start is called before the first frame update
    void Start()
    {
        foodBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        dragging = true;
        //distance between cooking item and the camera
        distance = Vector3.Distance(transform.position, Camera.main.transform.position);
        //store mouse pos
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 rayPoint = ray.GetPoint(distance);
        point = transform.position - rayPoint;
    }

    private void OnMouseDrag()
    {
        if (dragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 rayPoint = ray.GetPoint(distance);
            transform.position = rayPoint + point;
            

        }

    }


    private void OnMouseUp()
    {
        Debug.Log("mouse up");
        dragging = false;
        foodBody.isKinematic = false;
        
    }
}
