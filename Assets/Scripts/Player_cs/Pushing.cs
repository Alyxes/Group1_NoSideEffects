using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class Pushing : MonoBehaviour
    {
        public string furnitureName;
        public bool IsMovable = true;
        public bool IsMoving, isBeingPulled = false;
        public PlayerController player;
        public Transform playerPosition;

        private bool inZone = false;
        private Rigidbody furniture_rb;
        private InputAction interactButton;


        void Awake()
        {
            if (player == null)
                player = GetComponentInChildren<PlayerController>();

            playerPosition = player.transform;

            furniture_rb = GetComponent<Rigidbody>();
            interactButton = InputSystem.actions.FindAction("Interact");
        }

        // Update is called once per frame
        void Update()
        {
            if (!inZone)
                return;

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
                Vector3 newPos = Vector3.MoveTowards(furniture_rb.position, playerPosition.position, player.moveValue.sqrMagnitude * player.playerSpeed * Time.deltaTime);
                furniture_rb.MovePosition(newPos);
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
                    // EnableKinematic();
                    HUD.instance.ClearPickUpText();
                }
            }
        }
        //public void DisableKinematic()
        //{
        //    furniture_rb.isKinematic = false;
        //}
        //public void EnableKinematic()
        //{
        //    furniture_rb.isKinematic = true;
        //}
        public void EnablePulling()
        {
            if (!isBeingPulled)
            {
                isBeingPulled = true;
                player.playerSpeed = player.playerSpeed/4;
            }

            if (!IsMoving)
                IsMoving = true;
        }
        public void DisablePulling()
        {
            if (isBeingPulled)
            {
                isBeingPulled = false;
                player.playerSpeed = player.playerDaySpeed;
            }

            if (IsMoving)
                IsMoving = false;
        }
    }
}