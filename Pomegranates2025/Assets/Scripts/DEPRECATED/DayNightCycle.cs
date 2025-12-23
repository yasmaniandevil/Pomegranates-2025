using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class DayNightCycle : MonoBehaviour
{
    //a styruct is a unique data type
    [System.Serializable]
    public struct DayAndNightMark
    {
        public float timeRatio;
        public Color color;
        public float intensity;
    }

    [SerializeField] private DayAndNightMark[] marks;
    private float cycleLength = 24f; //in seconds

    //holds current time that has elapsed this cycle
    private float currentCycleTime;
    private int currentMarkIndex, nextMarkIndex;
    float currentMarkTime, nextMarkTime, marksTimeDifference;
    

    const float time_check = 0.1f;

    [SerializeField] private Light _light;
    [SerializeField] private Volume _volume;
    // Start is called before the first frame update
    void Start()
    {
        currentMarkIndex = -1;
        CycleMarks();
        
    }

    // Update is called once per frame
    void Update()
    {
        //total time elapsed since game started
        currentCycleTime = (currentCycleTime + Time.deltaTime) % cycleLength;

        //blend color and intensity
        float t = (currentCycleTime - currentMarkTime) / marksTimeDifference;
        DayAndNightMark current = marks[currentMarkIndex], next = marks[nextMarkIndex];
        _light.color = Color.Lerp(current.color, next.color, t);
        _light.intensity = Mathf.Lerp(current.intensity, next.intensity, t);
        //if passed mark
        //compare current cycle time var with time in secs of the next mark
        if(Mathf.Abs(currentCycleTime - nextMarkTime  ) < time_check)
        {
            
            _light.color = next.color;
            _light.intensity = next.intensity;

            CycleMarks();

        }
        
    }

    void CycleMarks()
    {
        //incre,emt thee index
        currentMarkIndex = (currentMarkIndex + 1) % marks.Length;
        nextMarkIndex = (currentMarkIndex + 1) % marks.Length;
        currentMarkTime = marks[currentMarkIndex].timeRatio * cycleLength;
        nextMarkTime = marks[nextMarkIndex].timeRatio * cycleLength;
        marksTimeDifference = nextMarkTime - currentMarkTime;
        if (marksTimeDifference < 0) marksTimeDifference += cycleLength;
    }
}
