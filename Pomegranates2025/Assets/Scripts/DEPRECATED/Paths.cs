using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/PathShape")]
public class Paths : ScriptableObject
{
    public Transform startingPos;
    public GameObject leftFoot;
    public GameObject rightFoot;
    public List<float> directionChanges;
    public float spacing = 1f;
    
}
