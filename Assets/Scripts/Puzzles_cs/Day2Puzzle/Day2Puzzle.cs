using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class SceneChecker : MonoBehaviour
    {
        public CombinationPickUp combinationItem;
        public PhoneScreen puzzle;
        public PlayerController Camera;
        [Header("UI Elements")]
        [SerializeField] private GameObject phoneButton;
        [SerializeField] private GameObject combination;

        private InputAction interactButton;
        private bool InZone, checkedPhoneOnce, enteredPhonePuzzle = false;

        // Names of the LostButtons that must be collected
        // [Header("Required LostButtons")]
        // [SerializeField] private string[] requiredButtons = { "PhoneButtonLost", "PhoneButtonLost (1)" };

        private void Awake()
        {
            // Hide UI elements at start
            if (phoneButton != null) phoneButton.SetActive(false);
            if (combination != null) combination.SetActive(false);

            // Get the global "Interact" action from Input System
            interactButton = InputSystem.actions.FindAction("Interact");
            //if (interactButton == null)
            //    Debug.LogError("SceneChecker: Could not find 'Interact' action! Make sure your Input Actions asset is set as Default.");
        }

        private void Update()
        {
            // if (interactButton == null) return;
            if (puzzle.isPuzzleDone)
            {
                if (InZone)
                    InZone = false;
                return;
            }

            if (InZone && interactButton.WasPressedThisFrame())
            {
                if (LostButtons.totalButtonsPickedUp == 2 && combinationItem.HasCombination)
                {
                    // Player has all buttons and the phone number card — show UI and allow interaction
                    HUD.instance.ClearPickUpText();
                    combination?.SetActive(combinationItem != null && combinationItem.HasCombination);
                    phoneButton?.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Camera.ToggleCameraRotationOff();
                    Camera.canMove = false;
                    if (!enteredPhonePuzzle)
                    {
                        StartCoroutine(HUD.instance.SetTimerUntilSubTitle(0.7f, "Whoah, that's... Jeezus, that's some weird handwriting there... \nCouldn't they have printed that number as well? They don't seem to want people to call...", true));
                        enteredPhonePuzzle = true;
                    }
                }
                else if (LostButtons.totalButtonsPickedUp < 2 && combinationItem.HasCombination)
                {
                    // Player has the phone number card but is missing buttons
                    if (!checkedPhoneOnce)
                    {
                        HUD.instance.SetSubTitleText("Some of the buttons are gone...? \n I have to find them too before I can call him...");
                        checkedPhoneOnce = true;
                    }
                    else
                        HUD.instance.SetSubTitleText("I still need to find the missing buttons.");

                }
                else if (LostButtons.totalButtonsPickedUp == 2 && !combinationItem.HasCombination)
                {
                    // Player is missing the phone number card but has all buttons
                    if (!checkedPhoneOnce)
                    {
                        HUD.instance.SetSubTitleText("Right. I need his card with the number.\nDidn't I put that on my desk?");
                        checkedPhoneOnce = true;
                    }
                    else
                        HUD.instance.SetSubTitleText("I still need to find his card. I think it's on my desk, by my flowers.");

                }
                else
                {
                    // Player is missing buttons
                    Debug.Log("You need to collect all LostButtons first!");
                    if (!checkedPhoneOnce)
                    {
                        HUD.instance.SetSubTitleText("Some of the buttons are gone...? \n I have to find them before I can call him.\nAnd I need his card, for the number.");
                        checkedPhoneOnce = true;
                    }
                    else
                        HUD.instance.SetSubTitleText("I still need to find the missing buttons and his card with the number.");

                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (puzzle.isPuzzleDone)
                return;

            if (other.CompareTag("InteractZone"))
            {
                InZone = true;
                if (!puzzle.isPuzzleDone)
                    HUD.instance.SetUniqueItemText("Call Dr. Raphael.");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (puzzle.isPuzzleDone)
                return;

            if (other.CompareTag("InteractZone"))
            {
                InZone = false;
                HUD.instance.ClearPickUpText();
                ClosePhoneUI();
            }
        }
        public void ClosePhoneUI()
        {
            phoneButton?.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            PlayerMovementEnabler(true);
        }
        public void PlayerMovementEnabler(bool enabled)
        {
            if (enabled)
                Camera.ToggleCameraRotationOn();
            else
                Camera.ToggleCameraRotationOff();

            Camera.canMove = enabled;
        }
    }
}
