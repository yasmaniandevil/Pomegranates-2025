using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

// ScriptableObject shows as an object reference which is good for larger projects
// System.Serializable is important to expose inline fields

[System.Serializable]
public class AudioClipExtended
{
    public string title;
    public AudioClip clip;

    [Range(0.0f, 1.0f)]
    public float volume;
    public bool loop;
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Audio Source")]
    [SerializeField] AudioSource musicSource; // gloabl
    [SerializeField] AudioSource sfxSource; // global

    [Header("Audio Clip")]
    public AudioClipExtended[] backgroundMusic;
    public AudioClipExtended[] soundFX;


    private Dictionary<string, AudioClipExtended> soundFXMap = new Dictionary<string, AudioClipExtended>();
    private Dictionary<string, AudioClipExtended> bgMap = new Dictionary<string, AudioClipExtended>();
    private AudioClipExtended currBG;
    private Coroutine _fadeCoroutine;
    private Coroutine _muteCoroutine;

    private float fadeTime;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        for (int i = 0; i < soundFX.Length; i++)
        {
            soundFXMap.Add(soundFX[i].title, soundFX[i]);
        }

        for (int i = 0; i < backgroundMusic.Length; i++)
        {
            bgMap.Add(backgroundMusic[i].title, backgroundMusic[i]);
        }

        fadeTime = 1.0f; // default
    }
    public void PlayGlobalSFXOneShot(string title)
    {
        if (!soundFXMap.TryGetValue(title, out AudioClipExtended audioSFXItem))
        {
            Debug.LogWarning($"SFX '{title}' not found!");
            return;
        }

        sfxSource.PlayOneShot(audioSFXItem.clip, audioSFXItem.volume);
    }

    public void PlayBGMusic(string title)
    {
        if (!bgMap.TryGetValue(title, out AudioClipExtended audioMusicItem))
        {
            Debug.LogWarning($"Music '{title}' not found!");
            return;
        }

        // Set the currBG - IMPORTANT
        currBG = audioMusicItem;

        // Fade In Ienumertor
        musicSource.clip = audioMusicItem.clip;
        musicSource.loop = audioMusicItem.loop;

        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(AudioFadeInBG(fadeTime));
    }

    public void StopBGMusic()
    {
        if (_fadeCoroutine != null)
        {
            StopCoroutine(_fadeCoroutine);
        }

        _fadeCoroutine = StartCoroutine(AudioFadeOutBG(fadeTime));
    }


    // Can't while loop time.deltaTime here since the former depends on frame by frame changes
    // Here we have no frame changes so we need to use a coroutine, otherwise a while loop alone will cause it to complete instantly
    public void MuteBGMusic(float muteFadeTime)
    {
        if (_muteCoroutine != null)
        {
            StopCoroutine(_muteCoroutine);
        }

        _muteCoroutine = StartCoroutine(MuteCoroutine(muteFadeTime));
    }
    IEnumerator MuteCoroutine(float muteFadeTime)
    {
        float t = 0f;
        float startVolume = musicSource.volume;

        while (t < muteFadeTime)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0.0f, t / muteFadeTime);

            yield return null; // wait one frame
        }
        musicSource.mute = true;
    }
    public void UnMuteBGMusic(float unMuteFadeTime)
    {
        if (_muteCoroutine != null)
        {
            StopCoroutine(_muteCoroutine);
        }

        _muteCoroutine = StartCoroutine(UnMuteCoroutine(unMuteFadeTime));
    }

    IEnumerator UnMuteCoroutine(float unMuteFadeTime)
    {
        musicSource.mute = false;
        float t = 0f;
        float targetVolume = currBG.volume;

        while (t < unMuteFadeTime)
        {
            t += Time.deltaTime;
            instance.musicSource.volume = Mathf.Lerp(0f, targetVolume, t / unMuteFadeTime);
            yield return null;
        }
    }


    public static IEnumerator AudioFadeInBG(float fadeTime = 5f)
    {
        instance.musicSource.volume = 0f;
        instance.musicSource.Play();

        float t = 0f;
        float targetVolume = instance.currBG.volume;

        while (t < fadeTime)
        {
            t += Time.deltaTime;

            // This is just linear interpolation over time
            // Core idea: Every frame we add a small fraction of the total change
            // Delta-time simply calculates time since last frame and fadeTime is the total time the fade should take
            // over fadeTime seconds, the sum of all deltaTime values should be equal to fadeTime 
            instance.musicSource.volume = Mathf.Lerp(0f, targetVolume, t / fadeTime);
            yield return null;
        }

        instance.musicSource.volume = targetVolume;
    }


    public static IEnumerator AudioFadeOutBG(float fadeTime = 5f)
    {
        float t = 0f;
        float startVolume = instance.musicSource.volume;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            instance.musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeTime);
            yield return null;
        }

        instance.musicSource.volume = 0f;
        instance.musicSource.Stop();
    }


    public void changeFadeTime(float newFadeTime)
    {
        fadeTime = newFadeTime;
    }


    /*
    // Variable dictates how fast the volume fades in :)
    // This default sets it to one second
    public static IEnumerator AudioFadeInBG(float fadeTime = 1.0f)
    {
        instance.musicSource.Play();
        float currVolume = instance.musicSource.volume;
        float targetVolume = instance.currBG.volume;

        while (currVolume <= targetVolume)
        {

            currVolume += targetVolume * (Time.deltaTime / fadeTime);
            instance.musicSource.volume = currVolume;
            yield return null;
        }

        instance.musicSource.volume = targetVolume;
    }
    */

}

