using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

namespace NoSideEffects
{
    public class HUD : MonoBehaviour
    {
        public static HUD instance;
        public TMP_Text pickUptext;
        public TMP_Text subTitleText;
        public TMP_Text pressToContinue;
        InputAction interactButton;
        public bool isSubTitleActive = false;
        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);

            interactButton = InputSystem.actions.FindAction("Interact");
        }

        public void SetSubTitleText(string input)
        {
            subTitleText.text = input;
            isSubTitleActive = true;
            pressToContinue.text = "[E] / [A]"; // We should make it so it differs depending on controller type used.
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

        void Update()
        {
            if (isSubTitleActive)
            {
                if (interactButton.WasPressedThisFrame())
                {
                    subTitleText.text = "";
                    pressToContinue.text = "";
                    isSubTitleActive = false;
                }
            }
        }
    }
}
