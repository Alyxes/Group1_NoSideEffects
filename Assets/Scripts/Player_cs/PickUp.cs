using NoSideEffects;
using UnityEngine;
using UnityEngine.InputSystem;

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

    private void Awake()
    {
        originalPosition = model.transform.position;
        originalRotation = model.transform.rotation;

        interactButton = InputSystem.actions.FindAction("Interact");
    }

    private void Update()
    {
        if (inZone && interactButton.WasPressedThisFrame())
        {
            if (!isHeld)
            {
                TryPickUp();
            }
            else
            {
                DropItem();
            }
        }
    }

    private void TryPickUp()
    {
        // Determine target hand and check if it's free
        Transform targetHoldPoint = PlayerInventory.holdPoint;
        bool handFree = false;

        if (itemID == "Flashlight" && PlayerInventory.holdPoint2 != null)
        {
            targetHoldPoint = PlayerInventory.holdPoint2;
            handFree = PlayerInventory.rightHandItem == null;
        }
        else
        {
            handFree = PlayerInventory.leftHandItem == null;
        }

        if (!handFree)
        {
            Debug.Log($"Cannot pick up {itemID}, hand is already occupied!");
            return;
        }

        // Attach item to hold point
        model.transform.SetParent(targetHoldPoint);
        model.transform.localPosition = Vector3.zero;
        model.transform.localRotation = Quaternion.Euler(pickupRotation);

        isHeld = true;

        // Assign to the correct hand
        if (itemID == "Flashlight")
            PlayerInventory.rightHandItem = this;
        else
            PlayerInventory.leftHandItem = this;

        HUD.instance.SetPutDownText(itemID);

        // Start task if applicable
        TaskSwitch taskSwitch = Object.FindFirstObjectByType<TaskSwitch>();
        if (taskSwitch != null)
            taskSwitch.StartTaskForCurrentItem();
    }

    private void DropItem()
    {
        // Clear hand reference
        if (itemID == "Flashlight")
            PlayerInventory.rightHandItem = null;
        else
            PlayerInventory.leftHandItem = null;

        isHeld = false;

        // Reset position and parent
        model.transform.SetParent(transform);
        model.transform.position = originalPosition;
        model.transform.rotation = originalRotation;

        HUD.instance.ClearPickUpText();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("InteractZone") && !inZone)
        {
            inZone = true;

            if (isHeld)
                HUD.instance.SetPutDownText(itemID);
            else
                HUD.instance.SetPickUpText(itemID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("InteractZone") && inZone)
        {
            inZone = false;
            HUD.instance.ClearPickUpText();
        }
    }
}
