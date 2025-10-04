using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioSource sfxSource;
    [SerializeField]
    private AudioClip currentClip;
    [SerializeField]
    private AudioClip[] soundEffects;
    [Range(0f, 1f)]
    [SerializeField]
    private float defaultVolume = 0.8f;

    private bool isMuted;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;
            audioSource.playOnAwake = false;
        }

        audioSource.volume = defaultVolume;
        if (currentClip != null)
        {
            PlayMusic(currentClip);
        }
    }

    public void PlayMusic(AudioClip clip, bool restartIfSame = false)
    {
        if (clip == null) return;

        if (!restartIfSame && audioSource.clip == clip && audioSource.isPlaying)
        {
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SetVolume(float volume01)
    {
        defaultVolume = Mathf.Clamp01(volume01);
        if (!isMuted)
        {
            audioSource.volume = defaultVolume;
        }
    }

    public void Mute(bool mute)
    {
        isMuted = mute;
        audioSource.volume = isMuted ? 0f : defaultVolume;
    }

    public bool IsPlaying()
    {
        return audioSource.isPlaying;
    }
}
