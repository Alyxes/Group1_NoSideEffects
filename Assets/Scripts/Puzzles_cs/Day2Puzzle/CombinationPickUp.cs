using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class CombinationPickUp : MonoBehaviour
    {
        public bool HasCombination = false;
        private bool InZone = false;

        private InputAction interactButton;

        private void Awake()
        {
            interactButton = InputSystem.actions.FindAction("Interact");
            //if (interactButton == null)
            //    Debug.LogError("SceneChecker: Could not find 'Interact' action! Make sure your Input Actions asset is set as Default.");
        }
        private void Update()
        {
            if (interactButton == null) return;

            if (InZone && interactButton.WasPressedThisFrame())
            {
                HasCombination = true;
                HUD.instance.ClearPickUpText();
                gameObject.SetActive(false);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                InZone = true;
                HUD.instance.SetUniqueItemText("Pick up phone number");
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                InZone = false;
                HUD.instance.ClearPickUpText();
            }
        }
    }
}
