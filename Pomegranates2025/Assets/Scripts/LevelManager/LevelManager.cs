using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private GameObject loaderCanvas;
    [SerializeField] private Image progressBar;
    private float target;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Subscribe to sceneLoaded event
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneCoroutine(sceneName));
    }

    IEnumerator LoadSceneCoroutine(string sceneName)
    {
        loaderCanvas.SetActive(true);
        target = 0;
        progressBar.fillAmount = 0;

        AsyncOperation scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        while (scene.progress < 0.9f)
        {
            target = scene.progress;
            yield return null;
        }

        target = 1f;

        // wait until progress bar visually fills
        while (progressBar.fillAmount < 0.99f)
        {
            yield return null;
        }

        loaderCanvas.SetActive(false);
        // DisableAllVolumes();
        scene.allowSceneActivation = true;
    }

    void Update()
    {
        progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, target, 3 * Time.deltaTime);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //  ResetLightingForScene();
    }

    void ResetLightingForScene()
    {
        Debug.Log("Scene Loaded Reset Lighting");
        // Update realtime GI environment so lighting refreshes correctly
        DynamicGI.UpdateEnvironment();

        // Reset ambient lighting to neutral gray to avoid leftover tints
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = Color.gray;

        // Reset reflection intensity to default
        RenderSettings.reflectionIntensity = 1f;
    }

    void DisableAllVolumes()
    {
        var volumes = FindObjectsOfType<UnityEngine.Rendering.Volume>();
        foreach (var v in volumes)
        {
            v.enabled = false;
        }
    }
}
