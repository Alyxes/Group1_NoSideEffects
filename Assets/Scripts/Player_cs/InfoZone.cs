using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class InfoZone : MonoBehaviour
    {
        [Header("Zone Data")]
        public string objectID;
        public string infoText;
        public bool oneTimeUse = false;

        private bool inZone = false;
        private InputAction interactButton;

        private void Awake()
        {
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        private void Update()
        {
            if (inZone && interactButton.WasPressedThisFrame())
            {
                Debug.Log("You did press interact...");
                HUD.instance.SetSubTitleText(infoText);

                if (oneTimeUse)
                {
                    inZone = false;
                    HUD.instance.ClearPickUpText();
                    gameObject.SetActive(false);
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (!inZone)
                {
                    inZone = true;

                    HUD.instance.SetUniqueItemText("Look at " + objectID);
                }
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (inZone)
                {
                    inZone = false;
                    HUD.instance.ClearPickUpText();
                }
            }
        }
    }
}
