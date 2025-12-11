using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Item Data")]
        public string itemID;
        public GameObject model;

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
                }
                else if (PlayerInventory.currentHeldItem == this)
                {
                    DropItem();
                }
                else
                {
                    Debug.Log("You're already holding something else!");
                }
            }
        }
        private void PickUpItem()
        {
            if (PlayerInventory.currentHeldItem != null || holdPoint == null || isHeld)
                return;

            PlayerInventory.currentHeldItem = this;
            isHeld = true;

            // -----------------------------
            // Select correct hold point
            // -----------------------------
            Transform targetHoldPoint = PlayerInventory.holdPoint;

            // If item is "Flashlight" → use HoldPoint2
            if (itemID == "Flashlight" && PlayerInventory.holdPoint2 != null)
            {
                targetHoldPoint = PlayerInventory.holdPoint2;
            }

            // Move model to the selected hold point
            model.transform.SetParent(targetHoldPoint);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.Euler(pickupRotation);

            HUD.instance.SetPutDownText(itemID);

            // Start Task in TaskSwitch
            TaskSwitch taskSwitch = Object.FindFirstObjectByType<TaskSwitch>();
            if (taskSwitch != null)
            {
                taskSwitch.StartTaskForCurrentItem();
            }
        }


        private void DropItem()
        {
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