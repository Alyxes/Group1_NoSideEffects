using System.Collections;
using System.Threading;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NoSideEffects
{
    public class StartScreen : MonoBehaviour
    {
        public Image logoScreen;
        public RawImage blackScreen;
        private float timer;
        private float blackAlpha;

        void Start()
        {
            AudioManager.StartLoopingSound(SoundType.TITLESONG, AudioManager.instance.audSrc_Music);
        }
        public void StartGame()
        {
            AudioManager.StopSound(AudioManager.instance.audSrc_Music, true, 5f);
            // Stop title music and play button click sound
            AudioManager.PlaySound(SoundType.BUTTONCLICK, AudioManager.instance.audSrc_InteractSound);

            StartCoroutine(WaitAndLoadScene("Day1"));
        }
        public void CreditsPage()
        {
            StartCoroutine(WaitAndLoadScene("CreditsPage"));
        }
        private IEnumerator WaitAndLoadScene(string scene)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            SceneManager.LoadScene(scene);
        }
        public void ExitGame()
        {
            AudioManager.StopSound(AudioManager.instance.audSrc_Music);
            StartCoroutine(WaitAndQuitGame());
        }
        private IEnumerator WaitAndQuitGame()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
            #else
                Application.Quit();
            #endif
        }
    }
}