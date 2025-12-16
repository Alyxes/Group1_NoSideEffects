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
    private TalkState talkStake = TalkState.None;

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
            DoAction();
            currentSequence.Clear(); // Reset for next attempt
        }
    }

    void DoAction()
    {
        Debug.Log("Success! Action triggered!");
        switch (talkStake)
        {
            case TalkState.None:
                HUD.instance.SetSubTitleText("Doctor- Hello How are you?");
                talkStake = TalkState.StepOne;
                break;
            case TalkState.StepOne:
                HUD.instance.SetSubTitleText("Doctor- I see you are awake. Can you hear me?");
                talkStake = TalkState.StepTwo;
                break;
        }
    }
}
