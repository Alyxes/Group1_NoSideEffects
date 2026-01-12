using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class LetterScreen : MonoBehaviour
    {
        private float timer = 2f;
        public TMP_Text continueText;
        private InputAction interactButton;
        private bool canPressContinue = false;

        void Awake()
        {
            continueText.gameObject.SetActive(false);
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        void Start()
        {
            AudioManager.StopSound(AudioManager.instance.audSrc_Music);
            StartCoroutine(ShowContinueTextAfterDelay());
            // AudioManager.PlaySound(SoundType.OPENLETTER, AudioManager.instance.audSrc_InteractSound); 
            AudioManager.PlaySound(SoundType.DOCTORLETTER, AudioManager.instance.audSrc_OtherVoice);
        }
        void Update()
        {
            if (canPressContinue && interactButton.WasPressedThisFrame())
            {
                AudioManager.StopSound(AudioManager.instance.audSrc_Music, true, 3f);
                AudioManager.StopSound(AudioManager.instance.audSrc_OtherVoice, true, 2f);
                StartCoroutine(DSM.instance.WaitAndStartNewDay(DSM.Days.Day1, 1f));
            }
        }
        public IEnumerator ShowContinueTextAfterDelay()
        {
            yield return new WaitForSeconds(timer);
            continueText.gameObject.SetActive(true);
            canPressContinue = true;
        }
    }
}
