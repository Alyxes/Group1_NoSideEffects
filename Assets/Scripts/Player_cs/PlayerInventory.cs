using NoSideEffects;
using UnityEngine;

namespace NoSideEffects
{
    public class PlayerInventory : MonoBehaviour
    {
        // Currently held item (static so ItemPickup can access it)
        public static ItemPickup currentHeldItem;
        public static ItemPickup currentHeldItemLeft;
        // Hold points
        public static Transform holdPoint;
        public static Transform holdPoint2; // ADDED

        [Header("Hold Point References")]
        [SerializeField] private Transform holdPointReference;   // Assign in Inspector
        [SerializeField] private Transform holdPoint2Reference;  // ADDED: Assign second hold point in Inspector

        [Header("Reference to TaskSwitch")]
        [SerializeField] private TaskSwitch taskSwitch; // Assign in Inspector

        // Property for currently held item's ID
        public string HeldItemID
        {
            get
            {
                if (currentHeldItem != null)
                    return currentHeldItem.itemID;
                else
                    return null;
            }

        }

        private void Awake()
        {
            // Assign static hold point from inspector reference
            if (holdPointReference != null)
            {
                holdPoint = holdPointReference;
            }
            else
            {
                Debug.LogError("PlayerInventory: HoldPoint not assigned in Inspector!");
            }

            // ADDED: Assign second hold point
            if (holdPoint2Reference != null)
            {
                holdPoint2 = holdPoint2Reference;
            }
            else
            {
                Debug.LogWarning("PlayerInventory: HoldPoint2 not assigned (optional).");
            }
        }

        public void PickUpItem(ItemPickup pickedUpItem)
        {
            if (pickedUpItem == null) return;

            currentHeldItem = pickedUpItem;

            // Attach item to hold point
            if (holdPoint != null)
            {
                pickedUpItem.transform.SetParent(holdPoint);
                pickedUpItem.transform.localPosition = Vector3.zero;
                pickedUpItem.transform.localRotation = Quaternion.identity;
            }

            Debug.Log("Picked up: " + pickedUpItem.itemID);

            // Start task in TaskSwitch automatically
            if (taskSwitch != null)
            {
                taskSwitch.playerInventory = this;
                taskSwitch.StartTaskForCurrentItem();
            }
        }

        public void DropItem()
        {
            if (currentHeldItem != null)
            {
                currentHeldItem.transform.SetParent(null);
                currentHeldItem = null;
            }
        }
    }
}
