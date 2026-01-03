using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : MonoBehaviour
{
    private PlayableDirector playableDirector;

    public TimelineAsset[] timelineAssets;
    private int timelineAssetInd;

    void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
        timelineAssetInd = 0;
    }
    // Start is called before the first frame update
    void Start()
    {
        playableDirector.Play(timelineAssets[timelineAssetInd]);
    }

    // Update is called once per frame
    void Update()
    {
        // None Here
    }

    public void PlayNext()
    {
        timelineAssetInd++;
        playableDirector.Play(timelineAssets[timelineAssetInd]);
    }

}
