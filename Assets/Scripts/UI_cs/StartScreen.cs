using System.Collections;
using UnityEditor;
using UnityEngine;
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

        private void Awake()
        {
            blackAlpha = 0f;
            blackScreen.color = new UnityEngine.Color(0, 0, 0, blackAlpha);
            logoScreen.color = new UnityEngine.Color(1, 1, 1, 1);
            timer = 5f;
        }
        void Start()
        {
            if (!AudioManager.instance.audSrc_Music.isPlaying)
                AudioManager.StartLoopingSound(SoundType.TITLESONG, AudioManager.instance.audSrc_Music);
            if (!DSM.instance.hasShownLogos)
                StartCoroutine(WaitAndHideLogoscreen());
            else
            {
                blackScreen.enabled = false;
                logoScreen.enabled = false;
            }
        }
        public void StartGame()
        {
            if (!DSM.instance.hasShownLogos)
                return;

            AudioManager.PlaySound(SoundType.BUTTONCLICK, AudioManager.instance.audSrc_InteractSound);

            StartCoroutine(WaitAndLoadScene("LetterScreen", 1f));
        }
        public void CreditsPage()
        {
            if (!DSM.instance.hasShownLogos)
                return;

            AudioManager.PlaySound(SoundType.BUTTONCLICK, AudioManager.instance.audSrc_InteractSound);

            StartCoroutine(WaitAndLoadScene("CreditsPage", 0.1f));
        }
        private IEnumerator WaitAndLoadScene(string scene, float timer)
        {
            yield return new WaitForSecondsRealtime(timer);
            SceneManager.LoadScene(scene);
        }
        public void ExitGame()
        {
            if (!DSM.instance.hasShownLogos)
                return;

            AudioManager.PlaySound(SoundType.BUTTONCLICK, AudioManager.instance.audSrc_InteractSound);

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
        public IEnumerator WaitAndHideLogoscreen()
        {
            yield return new WaitForSecondsRealtime(timer);
            while (blackAlpha < 1)
            {
                blackAlpha += Time.deltaTime * 2f;
                if (blackAlpha >= 1)
                {
                    blackAlpha = 1;
                    logoScreen.color = new UnityEngine.Color(1, 1, 1, 0);
                    logoScreen.enabled = false;
                    StartCoroutine(WaitAlittleMoreAndShowMenu());
                }
                blackScreen.color = new UnityEngine.Color(0, 0, 0, blackAlpha);
                yield return null;
            }
        }
        public IEnumerator WaitAlittleMoreAndShowMenu()
        {
            yield return new WaitForSecondsRealtime(1f);
            while (blackAlpha > 0)
            {
                blackAlpha -= Time.deltaTime * 5f;
                if (blackAlpha <= 0)
                {
                    blackAlpha = 0;
                    blackScreen.enabled = false;
                    DSM.instance.hasShownLogos = true;
                }
                blackScreen.color = new UnityEngine.Color(0, 0, 0, blackAlpha);
                yield return null;
            }
        }
    }
}