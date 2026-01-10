using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

namespace NoSideEffects
{
    public class LostButtons : MonoBehaviour
    {
        private bool inZone = false;
        private InputAction interactButton;
        public static int totalButtonsPickedUp = 0;

        // Static list to track picked-up objects
        public static List<string> pickedUpObjects = new List<string>();

        private void Awake()
        {
            interactButton = InputSystem.actions.FindAction("Interact");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                inZone = true;
                HUD.instance.SetUniqueItemText("Pick up phone button");
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                inZone = false;
                HUD.instance.ClearPickUpText();
            }
        }

        private void Update()
        {
            if (inZone && interactButton.WasPressedThisFrame())
            {
                Debug.Log(gameObject.name + " picked up!");

                totalButtonsPickedUp++;
                HUD.instance.ClearPickUpText();

                inZone = false;
                Destroy(gameObject);

                if (totalButtonsPickedUp == 1)
                    HUD.instance.SetSubTitleText("That's one...");
                else if (totalButtonsPickedUp == 2)
                    HUD.instance.SetSubTitleText("Two buttons found. Now where's the last one...?");
                else if (totalButtonsPickedUp == 3)
                    HUD.instance.SetSubTitleText("Ok, that's all of them.");
            }
        }
    }
}

