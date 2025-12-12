using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class Movable : MonoBehaviour
    {
        public string furnitureName;
        public bool IsMovable = true;
        public PlayerController player;
        public Transform playerPosition;

        private bool inZone, isBeingPulled, isPlayingSound = false;
        private Vector3 OldPos;
        private Rigidbody furniture_rb;
        private InputAction interactButton;


        void Awake()
        {
            if (player == null)
                player = GetComponentInChildren<PlayerController>();

            playerPosition = player.transform;

            furniture_rb = GetComponent<Rigidbody>();
            OldPos = furniture_rb.position;
            interactButton = InputSystem.actions.FindAction("Interact");
        }
        void Update()
        {
            if (inZone)
            {
                if (interactButton.IsPressed())
                {
                    if (IsMovable)
                    {
                        EnablePulling();
                        HUD.instance.SetUniqueItemText("Pulling " + furnitureName);
                    }
                }
                else
                {
                    DisablePulling();
                    HUD.instance.SetUniqueItemText("Move towards to push - Interact to pull " + furnitureName);
                }

                if (isBeingPulled)
                {
                    Vector3 newPos = Vector3.MoveTowards(furniture_rb.position, playerPosition.position, player.moveValue.sqrMagnitude / 1.7f * player.playerSpeed * Time.deltaTime);
                    furniture_rb.MovePosition(newPos);
                }
            }
            
            Vector3 currentPos = furniture_rb.position;

            if (currentPos != OldPos)
            {
                if (!isPlayingSound)
                {
                    if (AudioManager.instance.audSrc_MovingFurniture.isPlaying == false)
                    {
                        isPlayingSound = true;
                        Debug.Log("Scraping sound begin");
                        AudioManager.PlayFadeInSound(SoundType.FURNITUREMOVE, AudioManager.instance.audSrc_MovingFurniture, 0.2f, true);
                    }
                }

                Debug.Log("Scraping sound continues");
                OldPos = currentPos;
            }
            else
            {
                EndFurnitureSounds();
            }
            
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (!inZone)
                {
                    inZone = true;
                    HUD.instance.SetUniqueItemText("Move towards to push - Interact to pull " + furnitureName);
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
                    Debug.Log("left the pushing zone");
                    DisablePulling();
                    HUD.instance.ClearPickUpText();
                }
            }
        }
        public void EnablePulling()
        {
            if (!isBeingPulled)
            {
                isBeingPulled = true;
                player.playerSpeed = player.playerSpeed/2.3f;
            }
        }
        public void DisablePulling()
        {
            if (isBeingPulled)
            {
                isBeingPulled = false;
                player.playerSpeed = player.playerDaySpeed;
            }
        }
        private void EndFurnitureSounds()
        {
            if (isPlayingSound)
            {
                isPlayingSound = false;
                Debug.Log("Scraping sound stops.");
                AudioManager.StopSound(AudioManager.instance.audSrc_MovingFurniture, true, 0.3f);
            }
        }
    }
}