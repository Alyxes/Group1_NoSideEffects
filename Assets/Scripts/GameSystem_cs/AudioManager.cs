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
        FURNITUREMOVE
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
        public static void StartLoopingSound(SoundType sound, float volume = 1)
        {
            instance.audioSource.clip = instance.soundList[(int)sound];
            instance.audioSource.loop = true;
            instance.audioSource.volume = volume;
            instance.audioSource.Play();
        }
        // function to stop playing sound, giving option to fade out.
        public static void StopSound(bool fadeOut = false, float fadeDuration = 1f)
        {
            if (fadeOut)
            {
                instance.StartCoroutine(FadeOutSound(fadeDuration));
            }
            else
            {
                instance.audioSource.Stop();
                instance.audioSource.loop = false;
            }
        }
        private static IEnumerator FadeOutSound(float fadeDuration)
        {
            float startVolume = instance.audioSource.volume;
            float time = 0;
            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                instance.audioSource.volume = Mathf.Lerp(startVolume, 0, time / fadeDuration);
                yield return null;
            }
            instance.audioSource.Stop();
            instance.audioSource.loop = false;
            instance.audioSource.volume = startVolume; // Reset volume for future use
        }
    }
}
