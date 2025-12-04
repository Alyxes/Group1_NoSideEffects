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
        void Start()
        {
            AudioManager.StartLoopingSound(SoundType.TITLESONG);
        }
        public void StartGame()
        {
            AudioManager.StopSound(false, 1f);
            // Stop title music and play button click sound
            AudioManager.PlaySound(SoundType.BUTTONCLICK);

            StartCoroutine(WaitAndLoadScene("Day1"));
        }

        public IEnumerator WaitAndLoadScene(string scene)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            DSM.instance.StartingNewDay(DSM.Days.Day1);
        }
        public void ExitGame()
        {
            AudioManager.StopSound(false, 1f);
            StartCoroutine(WaitAndQuitGame());
        }
        public IEnumerator WaitAndQuitGame()
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