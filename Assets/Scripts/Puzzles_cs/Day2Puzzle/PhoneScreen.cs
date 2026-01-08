using NoSideEffects;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace NoSideEffects
{
    public class PhoneScreen : MonoBehaviour
    {
        private enum TalkState
        {
            None,
            StepOne,
            StepTwo,
            StepThree,
            StepFour,
            StepFive,
            Done
        }
        private TalkState talkState = TalkState.None;

        [Tooltip("Set the button IDs in the order they should be pressed")]
        public List<int> correctSequence = new List<int> { 2, 7, 10, 3, 9, 3, 1, 7, 12, 4 }; // fixed sequence
        private List<int> currentSequence = new List<int>();

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

        void OnButtonPressed(int pressedButtonId)
        {
            currentSequence.Add(pressedButtonId);
            Debug.Log("Pressed button ID: " + pressedButtonId);

            // Check if current sequence is correct so far
            for (int i = 0; i < currentSequence.Count; i++)
            {
                if (currentSequence[i] != correctSequence[i])
                {
                    // Konstig bugg här. Verkar inte få rätt siffra från knappen.
                    Debug.Log("Wrong sequence! Resetting.");
                    // Här bör vi kanske ändra? Ska man behöva skriva in hela längden innan man får ett svar? I så fall måste if-satsen längst ner också ändras.
                    HUD.instance.SetSubTitleText("That's not his number...");
                    currentSequence.Clear();
                    return;
                }
            }

            HUD.instance.SetSubTitleText("That's right it seems!");

            // Check if the full sequence is complete
            if (currentSequence.Count == correctSequence.Count)
            {
                Debug.Log("Sequence complete! Triggering action.");
                currentSequence.Clear(); // Reset for next attempt
                DoAction();
            }
        }
        void DoAction()
        {
            switch (talkState)
            {
                case TalkState.None:
                    HUD.instance.SetSubTitleText("Hello?... are you there?");
                    talkState = TalkState.StepOne;
                    Invoke(nameof(NextStep), 2f);
                    break;
            }
        }

        void NextStep()
        {
            switch (talkState)
            {
                case TalkState.StepOne:
                    HUD.instance.SetSubTitleText("Beep beep beep... \n Doctor- Sorry i can't pick up right now, send a message at the tone.");
                    talkState = TalkState.StepTwo;
                    Invoke(nameof(NextStep), 2f);
                    break;

                case TalkState.StepTwo:
                    HUD.instance.SetSubTitleText("Oh no what am i going to do, i need answers now!");
                    talkState = TalkState.StepThree;
                    DSM.instance.isDoneForTheDay = true;
                    break;
            }
        }

    }
}
