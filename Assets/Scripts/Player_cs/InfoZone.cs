using System;
using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class InfoZone : MonoBehaviour
    {
        [Header("Zone Data")]
        public string objectID;
        public string infoText;
        public bool oneTimeUse = false;
        public CameraTarget lookAtPoint;
        public PlayerController player;
        [NonSerialized] public bool hasBeenChecked = false;

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
                HUD.instance.SetSubTitleText(infoText);
                hasBeenChecked = true;

                player.MakePlayerLookAt(lookAtPoint);

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
