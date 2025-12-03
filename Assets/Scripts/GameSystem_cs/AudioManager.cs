using System.Collections;
using UnityEngine;

public enum SoundType
{
    TITLESONG,
    BUTTONCLICK,
    APARTMENTBUZZING,
    GETTINGUPFROMBED
}

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundList;

    public static AudioManager instance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundList[(int)sound], volume);
    }
    public void StartLoopingSound(SoundType sound, float volume = 1)
    {
        instance.audioSource.clip = instance.soundList[(int)sound];
        instance.audioSource.loop = true;
        instance.audioSource.volume = volume;
        instance.audioSource.Play();
    }
    // function to stop playing sound, giving option to fade out.
    public void StopSound(bool fadeOut = false, float fadeDuration = 1f)
    {
        if (fadeOut)
        {
            // Same until i make fade possible.
            instance.audioSource.Stop();
            instance.audioSource.loop = false;
        }
        else
        {
            instance.audioSource.Stop();
            instance.audioSource.loop = false;
        }
    }
}
