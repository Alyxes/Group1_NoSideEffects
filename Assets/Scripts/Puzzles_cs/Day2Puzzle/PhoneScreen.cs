using NoSideEffects;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

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
    public List<int> correctSequence = new List<int> { 1, 2, 3 }; // fixed sequence
    private List<int> currentSequence = new List<int>();

    void Start()
    {
        // Add listeners to all child buttons
        foreach (Transform child in transform)
        {
            Button button = child.GetComponent<Button>();
            ButtonID id = child.GetComponent<ButtonID>();
            if (button != null && id != null)
            {
                int capturedID = id.id; // capture for the lambda
                button.onClick.AddListener(() => OnButtonPressed(capturedID));
            }
        }
    }

    void OnButtonPressed(int buttonID)
    {
        currentSequence.Add(buttonID);
        Debug.Log("Pressed button ID: " + buttonID);

        // Check if current sequence is correct so far
        for (int i = 0; i < currentSequence.Count; i++)
        {
            if (currentSequence[i] != correctSequence[i])
            {
                Debug.Log("Wrong sequence! Resetting.");
                currentSequence.Clear();
                return;
            }
        }

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
                HUD.instance.SetSubTitleText("Doctor- Hello How are you?");
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
                HUD.instance.SetSubTitleText("Doctor- I see you are awake. Can you hear me?");
                talkState = TalkState.StepTwo;
                Invoke(nameof(NextStep), 2f);
                break;

            case TalkState.StepTwo:
                HUD.instance.SetSubTitleText("Not good, something weird is happening to me...");
                talkState = TalkState.StepThree;
                break;
        }
    }

}
