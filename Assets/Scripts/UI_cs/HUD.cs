using System;
using System.Collections;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Burst;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Windows;

namespace NoSideEffects
{
    public class HUD : MonoBehaviour
    {
        public static HUD instance;
        public TMP_Text pickUptext;
        public TMP_Text subTitleText;
        public TMP_Text pressToContinue;
        public RawImage blackScreen;
        InputAction interactButton;
        [NonSerialized] public bool isSubTitleActive, blackScreenFadeOut, blackScreenFadeIn = false;
        [NonSerialized] public float blackScreenFadeSpeed = 0;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            blackScreen.color = new Color(0, 0, 0, 0);
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        void Update()
        {
            if (isSubTitleActive)
            {
                if (interactButton.WasPressedThisFrame())
                {
                    ClearSubTitleText();
                }
            }

            if (blackScreenFadeOut)
            {
                FadeOutBlackScreen(blackScreenFadeSpeed);
            }
            if (blackScreenFadeIn)
            {
                FadeInBlackScreen(blackScreenFadeSpeed);
            }
        }

        public void SetSubTitleText(string input)
        {
            subTitleText.text = input;
            isSubTitleActive = true;
            pressToContinue.text = "(gamepad)A/(k&m)E/left mouseclick"; // We should make it so it differs depending on controller type used.
        }
        public void ClearSubTitleText()
        {
            subTitleText.text = "";
            pressToContinue.text = "";
            isSubTitleActive = false;
        }
        public void SetPickUpText(string itemName, string action = "Pick Up")
        {
            pickUptext.text = action + " " + itemName;
        }
        public void SetPutDownText(string itemName, string action = "Put Down")
        {
            pickUptext.text = action + " " + itemName;
        }
        public void SetUniqueItemText(string input)
        {
            pickUptext.text = input;
        }
        public void ClearPickUpText()
        {
            pickUptext.text = "";
        }
        public IEnumerator PickUpTimeOutCoroutine(float timer)
        {
            yield return new WaitForSeconds(timer);
            ClearPickUpText();
        }
        public IEnumerator SubTitleTimeOutCoroutine(float timer)
        {
            yield return new WaitForSeconds(timer);
            ClearSubTitleText();
        }
        public IEnumerator SetTimerUntilSubTitle(float timer, string text)
        {
            yield return new WaitForSeconds(timer);
            SetSubTitleText(text);
        }
        public IEnumerator SetTimerUntilItemText(float timer, string text)
        {
            yield return new WaitForSeconds(timer);
            SetUniqueItemText(text);
        }
        public void SetBlackScreenAlpha(float newAlpha)
        {
            blackScreen.color = new Color(0, 0, 0, newAlpha);
        }
        public void FadeOutBlackScreen(float fadeSpeed)
        {
            if (blackScreen.color.a > 0)
            {
                float newAlpha = blackScreen.color.a - Time.deltaTime * fadeSpeed;
                if (newAlpha < 0)
                {
                    newAlpha = 0;
                    blackScreenFadeOut = false;
                }
                blackScreen.color = new Color(0, 0, 0, newAlpha);
            }
        }
        public void FadeInBlackScreen(float fadeSpeed)
        {
            if (blackScreen.color.a < 1)
            {
                float newAlpha = blackScreen.color.a + Time.deltaTime * fadeSpeed;
                if (newAlpha > 1)
                {
                    newAlpha = 1;
                    blackScreenFadeIn = false;
                }
                blackScreen.color = new Color(0, 0, 0, newAlpha);
            }
        }
    }
}
