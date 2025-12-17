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
    public AudioClipExtended backgroundMusic;
    public AudioClipExtended[] soundFX;


    private Dictionary<string, AudioClipExtended> soundFXMap = new Dictionary<string, AudioClipExtended>();
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
    }
    public void PlayGlobalSFX(string title)
    {
        if (!soundFXMap.TryGetValue(title, out AudioClipExtended audioSFXItem))
        {
            Debug.LogWarning($"SFX '{title}' not found!");
            return;
        }

        sfxSource.PlayOneShot(audioSFXItem.clip, audioSFXItem.volume);
    }

    public void PlayBGMusic()
    {
        musicSource.clip = backgroundMusic.clip;
        musicSource.Play();
    }
}

