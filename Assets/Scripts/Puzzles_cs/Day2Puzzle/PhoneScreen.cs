using NoSideEffects;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;
using System;
using TMPro;

namespace NoSideEffects
{
    public class PhoneScreen : MonoBehaviour
    {
        private enum TalkState
        {
            None,
            StepOne,
            StepTwo,
            Done
        }
        private TalkState talkState = TalkState.None;
        public SceneChecker day2puzzleScript;
        public TMP_Text NumberAmount;

        [Tooltip("Set the button IDs in the order they should be pressed")]
        private List<int> correctSequence = new List<int> { 2, 7, 10, 3, 9, 3, 1, 7, 12, 4 }; // fixed sequence
        private List<int> currentSequence = new List<int>();

        private InputAction backButton;
        private bool hasEntered10numbers = false;
        [NonSerialized] public bool isPuzzleDone = false;

        private void Awake()
        {
            backButton = InputSystem.actions.FindAction("Back");
            NumberAmount.text = "0";
        }
        void Start()
        {
            // Add listeners to all child buttons
            foreach (Transform child in transform)
            {
                Button button = child.GetComponent<Button>();
                ButtonID Id = child.GetComponent<ButtonID>();
                if (button != null && Id != null)
                {
                    int capturedID = Id.id; // capture for the lambda
                    button.onClick.AddListener(() => OnButtonPressed(capturedID));
                }
                else
                {
                    Debug.Log("Button or ButtonID component missing on child: " + child.name);
                }
            }
        }

        private void Update()
        {
            if (backButton.WasPressedThisFrame())
            {
                day2puzzleScript.ClosePhoneUI();
            }
        }

        void OnButtonPressed(int pressedButtonId)
        {
            if (isPuzzleDone)
                return;

            if (!hasEntered10numbers && talkState == TalkState.None)
            {
                currentSequence.Add(pressedButtonId);
                Debug.Log("Pressed button ID: " + pressedButtonId);

                NumberAmount.text = currentSequence.Count.ToString();

                int rand = UnityEngine.Random.Range(0, 7);
                if (rand == 0)
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Beep*");
                else if (rand == 1)
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Boop*");
                else if (rand == 2)
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Bip*");
                else if (rand == 3)
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Peep*");
                else if (rand == 4)
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Podeep*");
                else if (rand == 5)
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Bopiddi*");
                else if (rand == 6)
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Plorp*\nUuhh... Yuck.");

                // StartCoroutine(HUD.instance.SubTitleTimeOutCoroutine(0.9f));

                if (currentSequence.Count == 10)
                {
                    hasEntered10numbers = true;
                    // Check if current sequence is correct.
                    for (int i = 0; i < currentSequence.Count; i++)
                    {
                        if (currentSequence[i] != correctSequence[i])
                        {
                            Debug.Log("Wrong sequence! Resetting.");
                            // Här bör vi kanske ändra? Ska man behöva skriva in hela längden innan man får ett svar? I så fall måste if-satsen längst ner också ändras.
                            currentSequence.Clear(); // Reset for next attempt
                            StartCoroutine(HUD.instance.SetTimerUntilSubTitle(0.5f, "*Beep - beep*\n\"The number you have dialed does not exist.\"", false));
                            StartCoroutine(HUD.instance.SetTimerUntilSubTitle(4.5f, "Ok, that wasn't right. Ugh... Should I even care to try this again?\n...Yeah, I should. I can't always bail out.", true));
                            StartCoroutine(WaitToTryAgain());
                            return;
                        }
                    }

                    // Current sequence is correct.
                    Debug.Log("Sequence complete! Triggering action.");
                    currentSequence.Clear();
                    day2puzzleScript.ClosePhoneUI();
                    day2puzzleScript.PlayerMovementEnabler(false);
                    Invoke(nameof(SwitchTalkState), 1.2f);
                    // SwitchTalkState();
                }
            }
        }
        public IEnumerator WaitToTryAgain()
        {
            yield return new WaitForSeconds(5.7f);
            NumberAmount.text = "0";
            hasEntered10numbers = false;
        }
        void SwitchTalkState()
        {
            switch (talkState)
            {
                case TalkState.None:
                    HUD.instance.SetSubTitleTextWithoutContinueText("I think it's connecting!");
                    talkState = TalkState.StepOne;
                    Invoke(nameof(SwitchTalkState), 2.3f);
                    break;
                case TalkState.StepOne:
                    HUD.instance.SetSubTitleTextWithoutContinueText("*Beep - beep - beep*\n\"Sorry, I can't pick up right now, please leave a message after the tone.\"\n*Beeeeeeep*");
                    isPuzzleDone = true;
                    talkState = TalkState.StepTwo;
                    Invoke(nameof(SwitchTalkState), 8f);
                    break;
                case TalkState.StepTwo:
                    HUD.instance.SetSubTitleText("*Click*\nThat's what I thought... He doesn't really want me to call him.\nI guess I'll just wait for them to reach out for results...");
                    day2puzzleScript.PlayerMovementEnabler(true);
                    DSM.instance.isDoneForTheDay = true;
                    talkState = TalkState.Done;
                    break;
            }
        }

        //void DoAction()
        //{
        //    switch (talkState)
        //    {
        //        case TalkState.None:
        //            talkState = TalkState.StepOne;
        //            Invoke(nameof(NextStep), 3.7f);
        //            break;
        //    }
        //}

        //void NextStep()
        //{
        //    switch (talkState)
        //    {
        //        case TalkState.StepOne:
        //            HUD.instance.SetSubTitleText("*Beep - beep - beep*\n\"Sorry, I can't pick up right now, please leave a message after the tone.\"");
        //            talkState = TalkState.StepTwo;
        //            Invoke(nameof(NextStep), 7f);
        //            break;

        //        case TalkState.StepTwo:
        //            HUD.instance.SetSubTitleText("That's what I thought... He doesn't really want me to call him.\nI guess I'll just wait for them to reach out for results...");
        //            talkState = TalkState.Done;
        //            day2puzzleScript.PlayerMovementEnabler(true);
        //            DSM.instance.isDoneForTheDay = true;
        //            break;
        //    }
        //}
    }
}
