using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip currentClip;
    [SerializeField] private AudioClip[] soundEffects;
    [Range(0f, 1f)] [SerializeField] private float defaultVolume = 0.8f;

    [Header("Trade BGM")]
    [SerializeField] private AudioClip bgAudioClipOne;
    [SerializeField] private AudioClip bgAudioClipTwo;
    [SerializeField] private AudioClip bgAudioClipThree;

    [Header("Trade SFX")]
    [SerializeField] private AudioClip buyItemAudioClip;

    [Header("Vocal SFX")]
    [SerializeField] private AudioClip[] dragonVocalSoundEffects;
    [SerializeField] private AudioClip[] eapramVocalSoundEffects;
    [SerializeField] private AudioClip[] hectorVocalSoundEffects;
    [SerializeField] private AudioClip[] hestaVocalSoundEffects;
    [SerializeField] private AudioClip[] rufkinVocalSoundEffects;
    [SerializeField] private AudioClip[] sirElliotVocalSoundEffects;
    [SerializeField] private AudioClip[] tarqueVocalSoundEffects;
    [SerializeField] private AudioClip[] verellaSyllaVocalSoundEffects;
    [SerializeField] private AudioClip[] wrenVocalSoundEffects;

    private const string VOCAL_DRAGON = "dragon";
    private const string VOCAL_EAPRAM = "eapram";
    private const string VOCAL_HECTOR = "hector";
    private const string VOCAL_HESTA = "hesta";
    private const string VOCAL_RUFKIN = "rufkin";
    private const string VOCAL_SIR_ELLIOT = "sirelliot";
    private const string VOCAL_TARQUE = "tarque";
    private const string VOCAL_VERELLA_SYLLA = "verellasylla";
    private const string VOCAL_WREN = "wren";

    private bool isMuted;

    private int currentBGMIndex;
    private const int MAX_BGM_TRACKS = 3;
    private const int BGM_TRACK_ONE = 1;
    private const int BGM_TRACK_TWO = 2;
    private const int BGM_TRACK_THREE = 3;

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
            audioSource.loop = false;
            audioSource.playOnAwake = false;
        }

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        audioSource.volume = defaultVolume;
        if (currentClip != null)
        {
            PlayMusic(currentClip);
        }

        currentBGMIndex = BGM_TRACK_ONE;
}

    private void Update()
    {
        if (audioSource.isPlaying == false)
        {
            UpdateBGMTrack();
        }
    }

    private void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        currentClip = clip;
        audioSource.clip = currentClip;
        audioSource.Play();
    }

    private void PlaySFXMusic(AudioClip clip, bool restartIfSame = false)
    {
        if (clip == null)
        {
            return;
        }

        if (sfxSource == null)
        {
            return;
        }

        if (!restartIfSame && sfxSource.clip == clip && sfxSource.isPlaying)
        {
            return;
        }

        sfxSource.clip = clip;
        sfxSource.Play();
    }

    private AudioClip GetRandomVocalClip(AudioClip[] vocalAudioClipArray)
    {
        int randomIndex = Random.Range(0, vocalAudioClipArray.Length);
        return vocalAudioClipArray[randomIndex];
    }

    private void UpdateBGMTrack()
    {
        if (currentBGMIndex >= MAX_BGM_TRACKS)
        {
            currentBGMIndex = BGM_TRACK_ONE;
        }
        else
        {
            currentBGMIndex++;
        }

        switch (currentBGMIndex)
        {
            case BGM_TRACK_ONE:
                PlayMusic(bgAudioClipOne);
                break;
            case BGM_TRACK_TWO:
                PlayMusic(bgAudioClipTwo);
                break;
            case BGM_TRACK_THREE:
                PlayMusic(bgAudioClipThree);
                break;
            default:
                PlayMusic(bgAudioClipOne);
                break;
        }
    }

    public void PlayBuyItemSFX()
    {
        PlaySFXMusic(buyItemAudioClip, false);
    }

    public void PlayRandomVocal(string name)
    {
        string localizedName = name.ToLower(); // TODO: Remove whitespaces and combine to form one word

        switch (localizedName)
        {
            case VOCAL_DRAGON:
                PlaySFXMusic(GetRandomVocalClip(dragonVocalSoundEffects));
                break;
            case VOCAL_EAPRAM:
                PlaySFXMusic(GetRandomVocalClip(eapramVocalSoundEffects));
                break;
            case VOCAL_HECTOR:
                PlaySFXMusic(GetRandomVocalClip(hectorVocalSoundEffects));
                break;
            case VOCAL_HESTA:
                PlaySFXMusic(GetRandomVocalClip(hestaVocalSoundEffects));
                break;
            case VOCAL_RUFKIN:
                PlaySFXMusic(GetRandomVocalClip(rufkinVocalSoundEffects));
                break;
            case VOCAL_TARQUE:
                PlaySFXMusic(GetRandomVocalClip(tarqueVocalSoundEffects));
                break;
            case VOCAL_SIR_ELLIOT:
                PlaySFXMusic(GetRandomVocalClip(sirElliotVocalSoundEffects));
                break;
            case VOCAL_VERELLA_SYLLA:
                PlaySFXMusic(GetRandomVocalClip(verellaSyllaVocalSoundEffects));
                break;
            case VOCAL_WREN:
                PlaySFXMusic(GetRandomVocalClip(wrenVocalSoundEffects));
                break;
            default:
                PlaySFXMusic(GetRandomVocalClip(dragonVocalSoundEffects));
                break;
        }
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
