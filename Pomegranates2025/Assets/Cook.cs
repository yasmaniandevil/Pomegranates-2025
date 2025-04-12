using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{
    public Vector3 point;
    public bool dragging = false;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnMouseDown()
    {
        dragging = true;
        point = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDrag()
    {
        if (dragging)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = mousePosition + point;
            Debug.Log("why");

        }

    }


    private void OnMouseUp()
    {
        Debug.Log("mouse down");
        dragging = false;
        Debug.Log(dragging);
    }
}
