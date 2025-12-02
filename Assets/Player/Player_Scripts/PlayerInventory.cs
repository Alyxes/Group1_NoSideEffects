using UnityEngine;

namespace NoSideEffects
{
    public class PlayerInventory : MonoBehaviour
    {
        // Currently held item (static so ItemPickup can access it)
        public static ItemPickup currentHeldItem;

        // Hold point for attaching items in player's hand
        public static Transform holdPoint;

        [Header("Hold Point Reference")]
        [SerializeField] private Transform holdPointReference; // Assign in Inspector

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
        }

        /// <summary>
        /// Call this when the player picks up an item
        /// </summary>
        /// <param name="pickedUpItem">The item being picked up</param>
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
                taskSwitch.playerInventory = this;          // Pass this inventory to TaskSwitch
                taskSwitch.StartTaskForCurrentItem();       // Start the task for the held item
            }
        }

        /// <summary>
        /// Optional: drop currently held item
        /// </summary>
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
