using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NoSideEffects
{
    public class ItemPickup : MonoBehaviour
    {
        [Header("Item Data")]
        public string itemID;              // unique ID or name
        public GameObject model;           // visual model only (child object)

        private Vector3 originalPosition;
        private Quaternion originalRotation;

        private bool isHeld = false;
        private bool inZone = false;
        private InputAction interactButton;
        private Transform holdPoint;
        private Collider triggerCollider;

        private void Awake()
        {
            // Store original location
            originalPosition = model.transform.position;
            originalRotation = model.transform.rotation;

            // Get trigger collider
            triggerCollider = GetComponent<Collider>();

            interactButton = InputSystem.actions.FindAction("Interact");
        }

        private void Start()
        {
            // Get hold point from PlayerInventory
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

            // Move only the visual model
            model.transform.SetParent(holdPoint);
            model.transform.localPosition = Vector3.zero;
            model.transform.localRotation = Quaternion.identity;

            HUD.instance.SetPutDownText(itemID);
        }

        private void DropItem()
        {
            PlayerInventory.currentHeldItem = null;
            isHeld = false;

            // Move visual model back to root
            model.transform.SetParent(transform);
            model.transform.position = originalPosition;
            model.transform.rotation = originalRotation;

            HUD.instance.SetPickUpText(itemID);
            // Root trigger is still enabled, no need to touch it
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

