using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    public float moveSpeed;
    public Vector3 targetPos = new Vector3(5, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //move obj towards target pos
        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        
        if(transform.position == targetPos)
        {
            Debug.Log("reached position");
        }
    }
}
