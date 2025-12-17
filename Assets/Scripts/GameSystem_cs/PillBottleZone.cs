using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class PillBottleZone : MonoBehaviour
    {
        private bool inZone = false;
        public PlayerController player;
        private InputAction interactButton;
        private bool tookApill = false;

        private void Awake()
        {
            interactButton = InputSystem.actions.FindAction("Interact");
            gameObject.SetActive(false);
        }

        void Update()
        {
            if (!tookApill && inZone && interactButton.WasPressedThisFrame())
            {
                AudioManager.PlaySound(SoundType.BUTTONCLICK, AudioManager.instance.audSrc_InteractSound, 0.7f);

                if (DSM.instance.endDayMonologue != "")
                    HUD.instance.SetSubTitleText(DSM.instance.endDayMonologue);

                player.ToggleCameraRotationOff();
                player.canMove = false;

                player.wantedHeadXrotation = 0f;
                player.wantedHeadYrotation = 180f;
                player.wantedXposition = 0f;

                tookApill = true;

                StartCoroutine(DSM.instance.WaitAndStartNewDay(DSM.instance.nextDay, 5f));
            }

            if (tookApill)
            {
                player.GoToBedAnimation();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (!inZone)
                {
                    inZone = true;

                    HUD.instance.SetUniqueItemText("Take a pill and go to sleep");
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
