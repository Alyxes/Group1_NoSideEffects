using System.Collections;
using UnityEngine;

namespace NoSideEffects
{
    public enum SoundType
    {
        TITLESONG,
        BUTTONCLICK,
        APARTMENTBUZZING,
        GETTINGUPFROMBED,
        DOORCLOSE,
        FURNITUREMOVE,
        PILLRATTLE
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioClip[] soundList;

        public static AudioManager instance;

        // Sound channels kinda...
        public AudioSource audSrc_Music;
        public AudioSource audSrc_ApartmentNoise;
        public AudioSource audSrc_NeighbourNoise;
        public AudioSource audSrc_WindowNoise;
        public AudioSource audSrc_FanNoise;
        public AudioSource audSrc_MovingFurniture;
        public AudioSource audSrc_DoorSound;
        public AudioSource audSrc_InteractSound;
        public AudioSource audSrc_PlayerVoice;
        public AudioSource audSrc_PlayerMovement;
        public AudioSource audSrc_OtherVoice;
        public AudioSource audSrc_NoisyVoices;
        public AudioSource audSrc_SuddenSound;
        public AudioSource audSrc_OtherSound;
        public AudioSource audSrc_DistantSound;

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
                return;
            }

            audSrc_Music = GetComponent<AudioSource>();
            audSrc_ApartmentNoise = GetComponent<AudioSource>();
            audSrc_NeighbourNoise = GetComponent<AudioSource>();
            audSrc_WindowNoise = GetComponent<AudioSource>();
            audSrc_FanNoise = GetComponent<AudioSource>();
            audSrc_MovingFurniture = GetComponent<AudioSource>();
            audSrc_DoorSound = GetComponent<AudioSource>();
            audSrc_InteractSound = GetComponent<AudioSource>();
            audSrc_PlayerVoice = GetComponent<AudioSource>();
            audSrc_PlayerMovement = GetComponent<AudioSource>();
            audSrc_OtherVoice = GetComponent<AudioSource>();
            audSrc_NoisyVoices = GetComponent<AudioSource>();
            audSrc_SuddenSound = GetComponent<AudioSource>();
            audSrc_OtherSound = GetComponent<AudioSource>();
            audSrc_DistantSound = GetComponent<AudioSource>();
        }
        public static void PlaySound(SoundType sound, AudioSource audioSource, float _volume = 1)
        {
            audioSource.volume = _volume;
            audioSource.PlayOneShot(instance.soundList[(int)sound], _volume);
        }
        public static void StartLoopingSound(SoundType sound, AudioSource audioSource, float _volume = 1)
        {
            audioSource.clip = instance.soundList[(int)sound];
            audioSource.loop = true;
            audioSource.volume = _volume;
            audioSource.Play();
        }
        // function to stop playing sound, giving option to fade out.
        public static void StopSound(AudioSource audioSource, bool fadeOut = false, float fadeDuration = 1f)
        {
            if (fadeOut)
            {
                instance.StartCoroutine(FadeOutSound(audioSource, fadeDuration));
            }
            else
            {
                audioSource.Stop();
                audioSource.loop = false;
                audioSource.volume = 0;
            }
        }
        public static void PlayFadeInSound(SoundType sound, AudioSource audioSource, float fadeInDuration, bool isLooping = true, float startVolume = 0f, float goalVolume = 1f)
        {
            audioSource.clip = instance.soundList[(int)sound];
            audioSource.volume = startVolume;

            if (isLooping)
            {
                audioSource.loop = true;
                audioSource.Play();
            }
            else
            {
                audioSource.PlayOneShot(instance.soundList[(int)sound], audioSource.volume);
            }

            instance.StartCoroutine(FadeInSound(audioSource, fadeInDuration, goalVolume));
        }
        public static IEnumerator FadeOutSound(AudioSource audioSource, float fadeDuration)
        {
            float startVolume = audioSource.volume;
            float time = 0;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0, time / fadeDuration);
                yield return null;
            }
            audioSource.Stop();
            audioSource.loop = false;
        }
        public static IEnumerator FadeInSound(AudioSource audioSource, float fadeInDuration, float goalVolume = 1f)
        {
            float finalVolume = goalVolume;
            float time = 0;
            while (time < fadeInDuration)
            {
                time += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(audioSource.volume, finalVolume, time / fadeInDuration);
                yield return null;
            }
        }
    }
}
