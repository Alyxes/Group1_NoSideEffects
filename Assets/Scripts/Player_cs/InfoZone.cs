using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class InfoZone : MonoBehaviour
    {
        [Header("Zone Data")]
        public string objectID;
        public string infoText;
        public PlayerController player;
        public Transform objectToLookAt;
        public bool oneTimeUse = false;
        public bool lockPlayerView = false;
        public float lockViewTime = 0f;
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

                if (lockPlayerView)
                {
                    player.ToggleCameraRotationOff();
                    StartCoroutine(player.SwitchCameraRotationOnTimer(lockViewTime));
                    player.cameraControl.SetHeadRotation(90f, 90f);
                }

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
