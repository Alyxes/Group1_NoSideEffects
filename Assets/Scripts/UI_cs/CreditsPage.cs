using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NoSideEffects
{
    public class CreditsPage : MonoBehaviour
    {
        private IEnumerator WaitAndLoadScene(string scene)
        {
            yield return new WaitForSecondsRealtime(0.1f);
            SceneManager.LoadScene(scene);
        }
        public void BackToStart()
        {
            StartCoroutine(WaitAndLoadScene("StartScreen"));
        }
    }
}