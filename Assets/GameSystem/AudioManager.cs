using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource sfxSource;

    public AudioClip StartScreenMusic;
    public AudioClip buttonClick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        PlayLoopingSound(StartScreenMusic);
    }
    
    public void PlayLoopingSound(AudioClip clip)
    {   
        sfxSource.clip = clip;
        sfxSource.loop = true;
        sfxSource.Play();
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
    public void SetSoundVolume(float volume)
    {
        sfxSource.volume = volume;
    }
}
