using System.Threading;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace NoSideEffects
{
    public class StartScreen : MonoBehaviour
    {
        public void StartGame()
        {
            StartCoroutine(WaitAndLoadScene("Day1"));
        }

        public IEnumerator WaitAndLoadScene(string scene)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            DSM.instance.StartingNewDay(DSM.Days.Day1);
        }
        public void ExitGame()
        {
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