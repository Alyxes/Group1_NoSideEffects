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
            SceneManager.LoadScene(scene);
        }
        public void ExitGame()
        {
            StartCoroutine(WaitAndQuitGame());
        }
        public IEnumerator WaitAndQuitGame()
        {
            yield return new WaitForSecondsRealtime(0.2f);
            #if UNITY_EDITOR
                EditorApplication.ExitPlaymode();
            #else
                Application.Quit();
            #endif
        }
    }
}
