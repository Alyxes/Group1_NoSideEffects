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
        private bool HasCombination = false;

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

            if (interactButton.WasPressedThisFrame())
            {
                if (InZone)
                {
                    if (combinationItem.HasCombination)
                    {
                        combination?.SetActive(true);
                    }
                    phoneButton?.SetActive(true);
                    Cursor.lockState = CursorLockMode.None;
                    Camera.ToggleCameraRotationOff();
                }
                else
                {
                    phoneButton?.SetActive(false);
                    Cursor.lockState = CursorLockMode.Locked;
                    Camera.ToggleCameraRotationOn();
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
            }
        }
    }
}

