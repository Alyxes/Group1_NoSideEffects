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
        public Image SubTiltleBGimage;
        InputAction interactButton;
        [NonSerialized] public bool isSubTitleActive, isPressToContinueActive, blackScreenFadeOut, blackScreenFadeIn = false;
        [NonSerialized] public float blackScreenFadeSpeed = 0;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            blackScreen.color = new Color(0, 0, 0, 0);
            SubTiltleBGimage.enabled = false;
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        void Update()
        {
            if (blackScreenFadeOut)
            {
                FadeOutBlackScreen(blackScreenFadeSpeed);
            }
            if (blackScreenFadeIn)
            {
                FadeInBlackScreen(blackScreenFadeSpeed);
            }
        }
        private void FixedUpdate()
        {
            if (isPressToContinueActive)
            {
                if (interactButton.WasPressedThisFrame())
                {
                    Debug.Log("clear subtitle");
                    ClearSubTitleText();
                }
            }
        }

        public void SetSubTitleText(string input)
        {
            if (isSubTitleActive)
            {
                ClearSubTitleText();
            }

            subTitleText.text = input;
            SetSubTitleActiveTrue();

            StartCoroutine(SubTitleBooleanTrueCoroutine());
        }
        public void SetSubTitleTextWithoutContinueText(string input)
        {
            if (isSubTitleActive)
            {
                ClearSubTitleText();
            }

            subTitleText.text = input;
            SetSubTitleActiveTrue();
        }

        public void ClearSubTitleText()
        {
            subTitleText.text = "";
            pressToContinue.text = "";
            SetSubTitleActiveFalse();
            isPressToContinueActive = false;
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
        private void SetSubTitleActiveTrue()
        {
            SubTiltleBGimage.enabled = true;
            isSubTitleActive = true;
        }
        private void SetSubTitleActiveFalse()
        {
            SubTiltleBGimage.enabled = false;
            isSubTitleActive = false;
        }
        public IEnumerator SubTitleBooleanTrueCoroutine()
        {
            yield return new WaitForSeconds(1f);
            if (isSubTitleActive)
            {
                isPressToContinueActive = true;
                pressToContinue.text = "(gamepad)A/(k&m)E/left mouseclick"; // We should make it so it differs depending on controller type used.
            }
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
        public IEnumerator SetTimerUntilSubTitle(float timer, string text, bool continueTextOn)
        {
            yield return new WaitForSeconds(timer);
            if (continueTextOn)
                SetSubTitleText(text);
            else
                SetSubTitleTextWithoutContinueText(text);
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
