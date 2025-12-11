using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Item Data")]
        public string itemID;
        public GameObject model;
        public PlayerController FlashLight;
        [Header("Pickup Rotation")]
        public Vector3 pickupRotation = Vector3.zero;

        private Vector3 originalPosition;
        private Quaternion originalRotation;

        private bool isHeld = false;
        private bool inZone = false;
        private InputAction interactButton;
        private Transform holdPoint;
        private Collider triggerCollider;

        private void Awake()
        {
            originalPosition = model.transform.position;
            originalRotation = model.transform.rotation;

            triggerCollider = GetComponent<Collider>();

            interactButton = InputSystem.actions.FindAction("Interact");
        }

        private void Start()
        {
            holdPoint = PlayerInventory.holdPoint;
            if (holdPoint == null)
                Debug.LogError("ItemPickup: No HoldPoint found in PlayerInventory!");
        }

        private void Update()
        {
            if (inZone && interactButton.WasPressedThisFrame())
            {
                if (PlayerInventory.currentHeldItem == null && !isHeld)
                {
                    PickUpItem();
                    if (PlayerInventory.currentHeldItemLeft != null &&
                        PlayerInventory.currentHeldItemLeft.itemID == "Flashlight")
                    {
                        FlashLight.FlashLight.SetActive(true);
                    }

                }
                else if (PlayerInventory.currentHeldItem == this ||
                        PlayerInventory.currentHeldItemLeft == this)
                {
                    DropItem();
                    if (itemID == "Flashlight")
                    {
                        FlashLight.FlashLight.SetActive(false);
                    }
                }

                else
                {
                    Debug.Log("You're already holding something else!");
                }
            }
        }
        private void PickUpItem()
        {
            bool isFlashlight = (itemID == "Flashlight");

            // BLOCK incorrect hand logic:
            if (isFlashlight)
            {
                // If flashlight already held, do nothing
                if (PlayerInventory.currentHeldItemLeft != null)
                    return;
            }
            else
            {
                // If holding another item in RIGHT hand, block pickup
                if (PlayerInventory.currentHeldItem != null)
                    return;
            }

            // Assign to correct hand
            if (isFlashlight)
                PlayerInventory.currentHeldItemLeft = this;
            else
                PlayerInventory.currentHeldItem = this;

            isHeld = true;

            // Choose correct hold point
            Transform targetHoldPoint = isFlashlight ?
                PlayerInventory.holdPoint2 :      // left hand
                PlayerInventory.holdPoint;        // right hand

            // Attach model to hand
            model.transform.SetParent(targetHoldPoint);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(pickupRotation);

            HUD.instance.SetPutDownText(itemID);

            // Only start tasks for RIGHT-HAND items
            if (!isFlashlight)
            {
                TaskSwitch taskSwitch = Object.FindFirstObjectByType<TaskSwitch>();
                if (taskSwitch != null)
                    taskSwitch.StartTaskForCurrentItem();
            }
        }



        private void DropItem()
        {
            bool isFlashlight = (itemID == "Flashlight");

            if (isFlashlight)
                PlayerInventory.currentHeldItemLeft = null;
            else
                PlayerInventory.currentHeldItem = null;

            isHeld = false;

            model.transform.SetParent(transform);
            model.transform.position = originalPosition;
            model.transform.rotation = originalRotation;
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("InteractZone"))
            {
                if (!inZone)
                {
                    inZone = true;

                    if (PlayerInventory.currentHeldItem == this)
                        HUD.instance.SetPutDownText(itemID);
                    else
                        HUD.instance.SetPickUpText(itemID);
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