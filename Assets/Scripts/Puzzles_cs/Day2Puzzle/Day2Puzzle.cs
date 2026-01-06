using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class SceneChecker : MonoBehaviour
    {
        public CombinationPickUp combinationItem;
        public PhoneScreen Button;
        public PlayerController Camera;
        [Header("UI Elements")]
        [SerializeField] private GameObject phoneButton;
        [SerializeField] private GameObject combination;

        private InputAction interactButton;
        private bool InZone = false;

        // Names of the LostButtons that must be collected
        [Header("Required LostButtons")]
        [SerializeField] private string[] requiredButtons = { "PhoneButtonLost", "PhoneButtonLost (1)" };

        private void Awake()
        {
            // Hide UI elements at start
            if (phoneButton != null) phoneButton.SetActive(false);
            if (combination != null) combination.SetActive(false);

            // Get the global "Interact" action from Input System
            interactButton = InputSystem.actions.FindAction("Interact");
            if (interactButton == null)
                Debug.LogError("SceneChecker: Could not find 'Interact' action! Make sure your Input Actions asset is set as Default.");
        }

        private void Update()
        {
            if (interactButton == null) return;

            if (interactButton.WasPressedThisFrame() && InZone)
            {
                // Check if player has collected all required LostButtons
                bool hasAllButtons = true;
                foreach (string buttonName in requiredButtons)
                {
                    if (!LostButtons.pickedUpObjects.Contains(buttonName))
                    {
                        hasAllButtons = false;
                        break;
                    }
                }

                if (hasAllButtons)
                {
                    // Player has all buttons — show UI and allow interaction
                    combination?.SetActive(combinationItem != null && combinationItem.HasCombination);
                    phoneButton?.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Camera.ToggleCameraRotationOff();
                }
                else
                {
                    // Player is missing buttons
                    Debug.Log("You need to collect all LostButtons first!");
                    HUD.instance.SetSubTitleText("Some of the buttons are gone..? \n I have to find them before i call Dr. Raphael.");
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
                InZone = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                InZone = false;
                phoneButton?.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Camera.ToggleCameraRotationOn();
            }
        }
    }
}
